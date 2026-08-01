using web_app_scratch.TCPServer;

namespace web_app_scratch.Middleware;

public delegate Task MiddlewareDelegate(RequestContext context, Func<RequestContext, Task> next);

public class PipelineBuilder
{
    private readonly List<MiddlewareDelegate> _middlewareDelegates = new();
    public PipelineBuilder Use(MiddlewareDelegate middleware)
    {
        _middlewareDelegates.Add(middleware);
        return this;
    }

    public Func<RequestContext, Task> Build(Func<RequestContext, Task> terminal)
    {
        Func<RequestContext, Task> pipeline = terminal;
        for(int i = _middlewareDelegates.Count - 1; i >= 0; i--)
        {
            var current = _middlewareDelegates[i];
            var next = pipeline;
            pipeline = context => current(context, next);
        }
        return pipeline;
    }
}