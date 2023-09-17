using System.Net.WebSockets;
using System.Net;
using System.Text;
using System.Timers;
using System.Text.Json;
using MeApuntoBackend.Services.Dto;
using MeApuntoBackend.Services.Enum;
using System.Net.Sockets;

namespace MeApuntoBackend.Services;
public class WebsocketHandler2 : BackgroundService
{
    //private static Dictionary<int, WebSocket> clients = new();
    //private static ExternalDeviceService? _externalDeviceService;

    //private static HttpListener _listener;
    //private readonly System.Timers.Timer _refreshTimer;
    //private readonly string URL = "http://127.0.0.1:5432/";
    //private readonly int secondsBetweenRefresh = 3;


    private static HttpListener httpListener;
    public WebsocketHandler2(IServiceProvider serviceProvider)
    {
        string[] prefixes = { "http://127.0.0.1:5432/" };
        httpListener = new HttpListener();
        foreach (string prefix in prefixes)
        {
            httpListener.Prefixes.Add(prefix);
        }

        httpListener.Start();
        Console.WriteLine("Server started. Listening for requests...");

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (true)
        {
            HttpListenerContext context = await httpListener.GetContextAsync();
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

        Console.WriteLine("WebSocket connected.");

        // Simulate sending messages to the client at regular intervals
        // Replace this with your actual logic for sending/receiving messages from clients
        _ = SendMessagesAsync(webSocket);

        // Handle incoming messages from the client
        await ReceiveMessagesAsync(webSocket);
    }

    private static async Task SendMessagesAsync(WebSocket webSocket)
    {
        try
        {
            while (webSocket.State == WebSocketState.Open)
            {
                string message = "Server message: " + DateTime.Now.ToString();
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);

                await webSocket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);
                await Task.Delay(5000); // Send a message every 5 seconds
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"WebSocket send error: {ex.Message}");
        }
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
}
