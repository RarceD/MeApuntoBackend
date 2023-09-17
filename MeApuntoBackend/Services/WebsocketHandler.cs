using System.Net.WebSockets;
using System.Net;
using System.Text;
using System.Timers;
using System.Text.Json;
using MeApuntoBackend.Services.Dto;
using MeApuntoBackend.Services.Enum;
using System.Net.Sockets;

namespace MeApuntoBackend.Services;
public class WebsocketHandler : BackgroundService
{
    private static Dictionary<int, WebSocket> clients = new();
    private static ExternalDeviceService? _externalDeviceService;
    private static HttpListener _httpListener = new();

    private readonly string URL = "http://127.0.0.1:5432/";
    private readonly int secondsBetweenRefresh = 3;
    private readonly System.Timers.Timer _refreshTimer;


    public WebsocketHandler(IServiceProvider serviceProvider)
    {
        string[] prefixes = { URL };
        foreach (string prefix in prefixes)
        {
            _httpListener.Prefixes.Add(prefix);
        }

        _httpListener.Start();
        Console.WriteLine("Server started. Listening for requests...");


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
    private static void TimerElapsed()
    {
        foreach (var client in clients)
        {
            WebSocket websocket = client.Value;
            if (websocket.State != WebSocketState.Open)
            {
                websocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                _ = clients.Remove(client.Key);
            }

            WSDeviceDto deviceInfo = new() { value = "deviceId" };
            WSInteractionDto receivedJsonInfo = new() { clientId = client.Key, type = WSMsgType.DEVICE_CTR, payload = deviceInfo };
            SendToFrontend(receivedJsonInfo, websocket);
        }
    }

    private static void SendToFrontend(WSInteractionDto data, WebSocket websocket)
    {
        string responseMessage = JsonSerializer.Serialize(data);
        byte[] responseBuffer = Encoding.UTF8.GetBytes(responseMessage);
        websocket.SendAsync(new ArraySegment<byte>(responseBuffer, 0, responseBuffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (true)
        {
            HttpListenerContext context = await _httpListener.GetContextAsync();
            if (context.Request.IsWebSocketRequest)
            {
                _ = HandleWebSocketAsync(context);
            }
            else
            {
                context.Response.StatusCode = 400;
                context.Response.Close();
            }
        }
    }
    private static async Task HandleWebSocketAsync(HttpListenerContext context)
    {
        HttpListenerWebSocketContext webSocketContext = await context.AcceptWebSocketAsync(null);
        WebSocket webSocket = webSocketContext.WebSocket;

        // Handle incoming messages from the client
        await ReceiveMessagesAsync(webSocket);
    }

    private static async Task ReceiveMessagesAsync(WebSocket webSocket)
    {
        try
        {
            while (webSocket.State == WebSocketState.Open)
            {
                Console.WriteLine("WebSocket received by client.");
                byte[] buffer = new byte[1024];
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                WSInteractionDto? receivedJsonInfo = JsonSerializer.Deserialize<WSInteractionDto>(receivedMessage);
                if (receivedJsonInfo == null) continue;
                if (receivedJsonInfo.type == WSMsgType.DEVICE_CTR)
                {
                    if (!clients.ContainsKey(receivedJsonInfo.clientId))
                        clients.Add(receivedJsonInfo.clientId, webSocket);
                    if (_externalDeviceService != null)
                    {
                        _ = Task.Run(() =>
                           _externalDeviceService.GetDeviceStatus(ResponseToClient, receivedJsonInfo)
                         );
                    }
                }


                if (result.MessageType == WebSocketMessageType.Close)
                {
                    Console.WriteLine("WebSocket closed by client.");
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"WebSocket receive error: {ex.Message}");
        }
    }
    private static void ResponseToClient(WSInteractionDto response)
    {
        var clientToSend = clients.Where(client => client.Key == response.clientId).FirstOrDefault();
        SendToFrontend(response, clientToSend.Value);
    }
}

