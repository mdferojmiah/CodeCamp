using System.Reflection;
using web_app_scratch.Attributes;
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

    public EndPoint MapPost(string path, Delegate handler)
    {
        return _router.MapPost(path, handler);
    }

    public MiniWebApplication MapControllers()
    {
        var controllers = Services.GetControllers();
        foreach(var controller in controllers)
        {
            var methods = controller.GetMethods();
            foreach (var method in methods)
            {
                var attr = method.GetCustomAttributes<HttpMethodAttribute>().FirstOrDefault();
                if(attr != null)
                {
                    if(attr.Method == "GET")
                    {
                        _router.MapGet(attr.Path, (RequestContext context) =>
                        {
                            var invoker = new HandlerInvoker(Services);
                            var instance = Activator.CreateInstance(controller);
                            var result = invoker.MethodInvoke(method, instance, context);
                            return result;
                        });
                    }else if(attr.Method == "POST")
                    {
                        _router.MapPost(attr.Path, (RequestContext context) =>
                        {
                            var invoker = new HandlerInvoker(Services);
                            var instance = Activator.CreateInstance(controller);
                            var result = invoker.MethodInvoke(method, instance, context);
                            return result;
                        });
                    }
                }
            }
        }
        return this;
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