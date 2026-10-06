using System.Collections.Concurrent;
using System.Net.WebSockets;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:6969");

var app = builder.Build();

app.UseWebSockets();

var _connections = new ConcurrentDictionary<string, WebSocket>();

app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var connection = await context.WebSockets.AcceptWebSocketAsync();
        string username = context.Request.Query["user"]!;

        if (string.IsNullOrWhiteSpace(username))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }

        _connections.TryAdd(username, connection);
        Console.WriteLine($"Websocket connection established for user: {username}");

        var buffer = new byte[1024];
        while (true)
        {
            var result = await connection.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if(result.MessageType == WebSocketMessageType.Close)
            {
                _connections.TryRemove(username, out _);
                Console.WriteLine($"Websocket connection removed for user: {username}");
                break;
            }

            if(result.MessageType == WebSocketMessageType.Text)
            {
                var message = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Received message from {username}: {message}");

                var spilted = message.Split(" ");
                var sender = spilted[0];
                var receiver = spilted[1];
                var msg = string.Join(" ", spilted, 2, spilted.Length - 2);

                Console.WriteLine($"Sender: {sender} | Receiver: {receiver} | Message: {msg}");

                var receiverConnection = _connections.TryGetValue(receiver, out var receiverSocket) ? receiverSocket : null;

                if(receiverConnection == null)
                {
                    Console.WriteLine($"{receiver} is not connected");
                    await connection.SendAsync(
                        System.Text.Encoding.UTF8.GetBytes($"{receiver} is not connected"),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None);
                }else
                {
                    var msgBytes = System.Text.Encoding.UTF8.GetBytes($"Message from {sender}: {msg}");
                    await receiverConnection.SendAsync(
                        new ArraySegment<byte>(msgBytes),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None);
                }
            }
        }
    }else
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
    }
});

app.Run();
