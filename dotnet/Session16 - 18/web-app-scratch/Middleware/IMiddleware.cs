using web_app_scratch.TCPServer;

namespace web_app_scratch.Middleware;

public interface IMiddleware
{
    Task InvokeAsync(RequestContext context, Func<RequestContext, Task> next);
}