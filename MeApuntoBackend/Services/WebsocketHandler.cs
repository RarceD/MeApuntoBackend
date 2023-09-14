using System.Net.WebSockets;
using System.Net;
using System.Text;
using System.Timers;
using System.Text.Json;
using MeApuntoBackend.Services.Dto;
using Newtonsoft.Json.Schema;
using MeApuntoBackend.Services.Enum;

namespace MeApuntoBackend.Services;
public class WebsocketHandler : BackgroundService
{
    private static Dictionary<int, WebSocket> clients = new();

    private readonly HttpListener _listener;
    private readonly System.Timers.Timer _refreshTimer;
    private readonly ExternalDeviceService _externalDeviceService;
    private readonly string URL = "http://127.0.0.1:5432/";
    private readonly int secondsBetweenRefresh = 3;

    public WebsocketHandler(IServiceProvider serviceProvider)
    {
        _listener = new HttpListener();
        _listener.Prefixes.Add(URL);
        _listener.Start();

        _refreshTimer = new(secondsBetweenRefresh * 1000);
        StartTimer();

        using (IServiceScope scope = serviceProvider.CreateScope())
        {
            _externalDeviceService = scope.ServiceProvider.GetRequiredService<ExternalDeviceService>();
        }
    }
    private void StartTimer()
    {
        _refreshTimer.Elapsed += (object? sender, ElapsedEventArgs events) => TimerElapsed();
        _refreshTimer.AutoReset = true;
        _refreshTimer.Start();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _ = Task.Run(async () =>
        {
            while (true)
            {
                HttpListenerContext context = await _listener.GetContextAsync();
                context.Response.KeepAlive = false; // This param is extremelly important
                if (context.Request.IsWebSocketRequest)
                {
                    _ = ProcessWebSocketRequest(context);
                }
            }
        });
        return Task.CompletedTask;
    }

    private static async Task ProcessWebSocketRequest(HttpListenerContext context)
    {
        HttpListenerWebSocketContext webSocketContext = await context.AcceptWebSocketAsync(null);
        WebSocket webSocket = webSocketContext.WebSocket;
        while (webSocket.State == WebSocketState.Open)
        {
            byte[] buffer = new byte[1024 * 2];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Text)
            {
                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                WSInteractionDto? receivedJsonInfo = JsonSerializer.Deserialize<WSInteractionDto>(receivedMessage);
                if (receivedJsonInfo == null) continue;

                // Store client who want it's device info only:
                if (receivedJsonInfo.type == WSMsgType.DEVICE_CTR)
                {
                    clients.Add(receivedJsonInfo.clientId, webSocket);
                }
            }
        }
    }

    private static void TimerElapsed()
    {
        foreach (var client in clients)
        {
            WebSocket websocket = client.Value;
            if (websocket.State != WebSocketState.Open)
            {
                websocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                clients.Remove(client.Key);
            }

            WSDeviceDto deviceInfo = new() { value  = "deviceId" };
            WSInteractionDto receivedJsonInfo = new() { clientId = client.Key, type = WSMsgType.DEVICE_CTR ,payload = deviceInfo };
            SendToFrontend(receivedJsonInfo, websocket);
        }
    }

    private static void SendToFrontend(WSInteractionDto data, WebSocket websocket)
    {
        string responseMessage = JsonSerializer.Serialize(data);
        byte[] responseBuffer = Encoding.UTF8.GetBytes(responseMessage);
        websocket.SendAsync(new ArraySegment<byte>(responseBuffer, 0, responseBuffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
    }
}
