using System.Net.WebSockets;
using System.Net;
using System.Text;
using System.Timers;
using System.Text.Json;
using MeApuntoBackend.Services.Dto;

namespace MeApuntoBackend.Services
{
    public class WebsocketHandler : BackgroundService
    {
        private static Dictionary<int, WebSocket> clients = new();
        private System.Timers.Timer? timer;
        private string URL = "http://127.0.0.1:5432/";

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Set timer to execute periodically:
            _ = Task.Run(() =>
            {
                Run();
            });
            return Task.CompletedTask;
        }

        public async Task Run()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(URL);
            listener.Start();
            Console.WriteLine("WebSocket server is running: " + URL);
            ;
            timer = new(10000);
            timer.Elapsed += TimerElapsed;
            timer.AutoReset = true;
            timer.Start();

            while (true)
            {
                HttpListenerContext context = await listener.GetContextAsync();
                context.Response.KeepAlive = false; // This param is extremelly important
                if (context.Request.IsWebSocketRequest)
                {
                    Console.WriteLine("Something happens");
                    _ = ProcessWebSocketRequest(context);
                }
            }

        }

        private static async Task ProcessWebSocketRequest(HttpListenerContext context)
        {
            HttpListenerWebSocketContext webSocketContext = await context.AcceptWebSocketAsync(null);
            WebSocket webSocket = webSocketContext.WebSocket;

            byte[] buffer = new byte[1024];

            while (webSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine($"Received: {receivedMessage}");

                    // You can process the received message here and send a response if needed.
                    ClientWSInteractionDto receivedJsonInfo = JsonSerializer.Deserialize<ClientWSInteractionDto>(receivedMessage);
                    Console.WriteLine($"client id: {receivedJsonInfo.clientId}");

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
            // This method will be called every 10 seconds
            Console.WriteLine("Timer elapsed at " + DateTime.Now);
            if (clients.Count > 0)
            {
                foreach (var client in clients)
                {

                    WebSocket ws = client.Value;
                    if (ws.State != WebSocketState.Open)
                    {
                        ws.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                        clients.Remove(client.Key);
                    }

                    Console.WriteLine(ws.State);
                    ClientWSInteractionDto receivedJsonInfo = new() { clientId = client.Key, valuesResponse = DateTime.Now.ToString() };
                    string responseMessage = JsonSerializer.Serialize(receivedJsonInfo);
                    byte[] responseBuffer = Encoding.UTF8.GetBytes(responseMessage);
                    ws.SendAsync(new ArraySegment<byte>(responseBuffer, 0, responseBuffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);

                }
            }
        }

    }
}

/*
internal class Program
{
    private static Dictionary<int, WebSocket> clients = new();
    private static System.Timers.Timer? timer;
    private static string URL = "http://127.0.0.1:5432/";
    static async Task Main(string[] args)
    {
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add(URL);
        listener.Start();
        Console.WriteLine("WebSocket server is running: " + URL);
        ;
        timer = new(10000);
        timer.Elapsed += TimerElapsed;
        timer.AutoReset = true;
        timer.Start();

        while (true)
        {
            HttpListenerContext context = await listener.GetContextAsync();
            context.Response.KeepAlive = false; // This param is extremelly important
            if (context.Request.IsWebSocketRequest)
            {
                Console.WriteLine("Something happens");
                _ = ProcessWebSocketRequest(context);
            }
        }

    }

    private static async Task ProcessWebSocketRequest(HttpListenerContext context)
    {
        HttpListenerWebSocketContext webSocketContext = await context.AcceptWebSocketAsync(null);
        WebSocket webSocket = webSocketContext.WebSocket;

        byte[] buffer = new byte[1024];

        while (webSocket.State == WebSocketState.Open)
        {
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Text)
            {
                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Received: {receivedMessage}");

                // You can process the received message here and send a response if needed.
                ClientWSInteraction receivedJsonInfo = JsonSerializer.Deserialize<ClientWSInteraction>(receivedMessage);
                Console.WriteLine($"client id: {receivedJsonInfo.clientId}");

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
        // This method will be called every 10 seconds
        Console.WriteLine("Timer elapsed at " + DateTime.Now);
        if (clients.Count > 0)
        {
            foreach (var client in clients)
            {

                WebSocket ws = client.Value;
                if (ws.State != WebSocketState.Open)
                {
                    ws.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                    clients.Remove(client.Key);
                }

                Console.WriteLine(ws.State);
                ClientWSInteraction receivedJsonInfo = new() { clientId = client.Key, valuesResponse = DateTime.Now.ToString() };
                string responseMessage = JsonSerializer.Serialize(receivedJsonInfo);
                byte[] responseBuffer = Encoding.UTF8.GetBytes(responseMessage);
                ws.SendAsync(new ArraySegment<byte>(responseBuffer, 0, responseBuffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);

            }
        }
    }
}
}
*/
