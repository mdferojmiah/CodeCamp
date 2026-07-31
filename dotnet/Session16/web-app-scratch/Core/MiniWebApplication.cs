using web_app_scratch.DI;
using web_app_scratch.TCPServer;

namespace web_app_scratch.Core;
internal class MiniWebApplication(ServiceProvider services)
{
    private readonly Router _router = new();
    public readonly ServiceProvider Services = services;
    public EndPoint MapGet(string path, Delegate handler)
    {
        return _router.MapGet(path, handler);
    }

    public async Task RunAsync(int port = 0)
    {
        var server = new TcpServer(port, _router);
        await server.StartAsync();
    }
}