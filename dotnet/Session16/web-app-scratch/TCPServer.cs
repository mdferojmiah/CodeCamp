using System.Net;
using System.Net.Sockets;
using System.Text;

namespace web_app_scratch;

public class RequestContext
{
    public string Method { get; set; } = string.Empty;
    public string  Path { get; set; } = string.Empty;
}

public class TCPServer
{
    private readonly int _port;

    public TCPServer(int port)
    {
        _port = port;
    }

    public async Task StartAsync()
    {
        var listener = new TcpListener(IPAddress.Any, _port);
        listener.Start();

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
        var buffer = new byte[1024];

        var byteCount = await stream.ReadAsync(buffer);
        var requestText = Encoding.UTF8.GetString(buffer, 0, byteCount);

        var lines = requestText.Split("\r\n");
        var requestLine =  lines[0].Split(' ');

        // var method = requestLine[0];
        // var path = requestLine[1];

        var context = new RequestContext
        {
            Method = requestLine[0],
            Path = requestLine[1]
        };

        var responseText = $"Received a {context.Method} request on {context.Path}";

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