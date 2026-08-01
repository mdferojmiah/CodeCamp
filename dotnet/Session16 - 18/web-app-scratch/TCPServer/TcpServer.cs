using System.Net;
using System.Net.Sockets;
using System.Text;
using web_app_scratch.Core;
using web_app_scratch.DI;
using web_app_scratch.ModelBinder;

namespace web_app_scratch.TCPServer;

public class RequestContext
{
    public string Method { get; set; } = string.Empty;
    public string  Path { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public Dictionary<string, string> Headers = [];
    public string? Body { get; set; }
    public string? Response { get; set; }
}

public class TcpServer
{
    private readonly int _port;
    private readonly Router _router;
    private readonly ServiceProvider _services;
    private readonly Func<RequestContext, Task> _pipeline;

    public TcpServer(int port, Router router, ServiceProvider services, Func<RequestContext, Task> pipeline)
    {
        _port = port;
        _router = router;
        _services = services;
        _pipeline = pipeline;
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

        // var invoker = new HandlerInvoker(_services);
        // var responseText = _router.Resolve(context, invoker);
        // var responseText = $"Received a {context.Method} request on {context.Path}";

        try
        {
            await _pipeline(context);
        }
        catch(HttpException ex)
        {
            await SendError(stream, ex.StatusCode, ex.Message);
            client.Close();
            return;
        }
        catch(Exception ex)
        {
            await SendError(stream, 500, ex.Message);
            client.Close();
            return;
        }

        var responseText = context.Response ?? "";

        var responseInByte = Encoding.UTF8.GetBytes(
            "HTTP/1.1 200 OK\r\n" +
            "Date: " + DateTime.UtcNow.ToString() + "\r\n" +
            "Content-Length: " + responseText.Length + "\r\n" +
            "X-Name: Feroj Miah\r\n\r\n" + 
            responseText
        );

        await stream.WriteAsync(responseInByte);
    }

    private async Task SendError(NetworkStream stream, int statusCode, string message)
    {
        var body = $"{statusCode} {message}";
        var response = Encoding.UTF8.GetBytes(
            $"HTTP/1.1 {statusCode} {GetStatusText(statusCode)}\r\n" +
            "Content-Length: " + body.Length + "\r\n\r\n" + 
            body
        );
        await stream.WriteAsync(response);
    }

    private static string GetStatusText(int statusCode) => statusCode switch 
    {
        400 => "Bad Request",
        413 => "Request Entity Too Large",
        500 => "Internal Server Error",
        _ => "Unknown"
    };
}