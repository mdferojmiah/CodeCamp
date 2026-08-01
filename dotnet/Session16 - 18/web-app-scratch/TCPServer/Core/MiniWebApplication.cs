using web_app_scratch.DI;
using web_app_scratch.Middleware;
using web_app_scratch.ModelBinder;
using web_app_scratch.TCPServer;

namespace web_app_scratch.Core;
internal class MiniWebApplication(ServiceProvider services)
{
    private readonly Router _router = new();
    public readonly ServiceProvider Services = services;
    private readonly PipelineBuilder _pipelineBuilder = new();

    public void Use(MiddlewareDelegate middleware)
    {
        _pipelineBuilder.Use(middleware);
    }

    public EndPoint MapGet(string path, Delegate handler)
    {
        return _router.MapGet(path, handler);
    }

    public async Task RunAsync(int port = 0)
    {
        Func<RequestContext, Task> terminal = async (context) =>
        {
            var invoker = new HandlerInvoker(Services);
            context.Response = _router.Resolve(context, invoker);
        };

        var pipeline =  _pipelineBuilder.Build(terminal);

        var server = new TcpServer(port, _router, Services, pipeline);
        await server.StartAsync();
    }
}