using System.Net;
using System.Net.Sockets;
using System.Text;
using web_app_scratch.Core;

namespace web_app_scratch.TCPServer;

public class RequestContext
{
    public string Method { get; set; } = string.Empty;
    public string  Path { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public Dictionary<string, string> Headers = [];
    public string? Body { get; set; }
}

public class TcpServer
{
    private readonly int _port;
    private readonly Router _router;

    public TcpServer(int port, Router router)
    {
        _port = port;
        _router = router;
    }

    public async Task StartAsync()
    {
        var listener = new TcpListener(IPAddress.Any, _port);
        listener.Start();

        Console.WriteLine($"Server is running on port: {((IPEndPoint)listener.LocalEndpoint).Port}");

        while (true)
        {
            var client = await listener.AcceptTcpClientAsync();
            _ = Task.Run(() => HandleClientAsync(client)); // for different thread
            // await HandleClientAsync(client);
        }
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        using var stream = client.GetStream();

        var (rawHeader, rawBody) = await HttpRequestReader.ReadAsync(stream);

        var context = HttpHeaderParser.Parse(rawHeader);
        context.Body = HttpBodyParser.Parse(rawBody);

        var responseText = _router.Resolve(context);

        // var responseText = $"Received a {context.Method} request on {context.Path}";

        var responseInByte = Encoding.UTF8.GetBytes(
            "HTTP/1.1 200 OK\r\n" +
            "Date: " + DateTime.UtcNow.ToString() + "\r\n" +
            "Content-Length: " + responseText.Length + "\r\n" +
            "X-Name: Feroj Miah\r\n\r\n" + 
            responseText
        );

        await stream.WriteAsync(responseInByte);
    }
}