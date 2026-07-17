namespace learning_middleware;

public delegate Task MiddlewareDelegate(HttpContext context, Func<HttpContext, Task> next);

public class PipelineBuilder
{
    private readonly List<MiddlewareDelegate> _middlewareDelegates = new();
    public PipelineBuilder Use(MiddlewareDelegate middleware)
    {
        _middlewareDelegates.Add(middleware);
        return this;
    }

    public Func<HttpContext, Task> Build()
    {
        Func<HttpContext, Task> pipeline = EndFunction;
        for(int i = _middlewareDelegates.Count - 1; i >= 0; i--)
        {
            var current = _middlewareDelegates[i];
            var next = pipeline;
            pipeline = context => current(context, next);
        }
        return pipeline;
    }

    public Task EndFunction(HttpContext context)
    {
        Console.WriteLine("End of Chain");
        return Task.CompletedTask;
    }
}