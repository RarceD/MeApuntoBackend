using System.Net.WebSockets;
using System.Net;
using System.Text;
using System.Timers;
using System.Text.Json;
using MeApuntoBackend.Services.Dto;

namespace MeApuntoBackend.Services;
public class WebsocketHandler : BackgroundService
{
    private static Dictionary<int, WebSocket> clients = new();
    private string URL = "http://127.0.0.1:5432/";

    private readonly System.Timers.Timer _refreshTimer;
    private readonly HttpListener _listener;

    public WebsocketHandler()
    {
        _listener = new HttpListener();
        _listener.Prefixes.Add(URL);
        _listener.Start();

        _refreshTimer = new(10000);
        StartTimer();
    }
    private void StartTimer()
    {
        _refreshTimer.Elapsed += TimerElapsed;
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
            byte[] buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Text)
            {
                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Received: {receivedMessage}");

                // You can process the received message here and send a response if needed.
                ClientWSInteractionDto? receivedJsonInfo = JsonSerializer.Deserialize<ClientWSInteractionDto>(receivedMessage);
                Console.WriteLine($"client id: {receivedJsonInfo?.clientId}");

                clients.Add(receivedJsonInfo.clientId, webSocket);


                // Example: Send a response
                string responseMessage = $"Server Received: {receivedMessage}";
                byte[] responseBuffer = Encoding.UTF8.GetBytes(responseMessage);
                await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer, 0, responseBuffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
    private static void TimerElapsed(object sender, ElapsedEventArgs e)
    {
        foreach (var client in clients)
        {
            WebSocket ws = client.Value;
            if (ws.State != WebSocketState.Open)
            {
                ws.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                clients.Remove(client.Key);
            }
            ClientWSInteractionDto receivedJsonInfo = new() { clientId = client.Key, valuesResponse = DateTime.Now.ToString() };
            SendToFrontend(receivedJsonInfo, ws);
        }
    }

    private static void SendToFrontend(ClientWSInteractionDto data, WebSocket ws)
    {
        string responseMessage = JsonSerializer.Serialize(data);
        byte[] responseBuffer = Encoding.UTF8.GetBytes(responseMessage);
        ws.SendAsync(new ArraySegment<byte>(responseBuffer, 0, responseBuffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
    }
}
