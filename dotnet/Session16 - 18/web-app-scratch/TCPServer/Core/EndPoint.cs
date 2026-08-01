using web_app_scratch.TCPServer;

namespace web_app_scratch.Core;

public class EndPoint(string path, string method, Delegate handler)
{
    public readonly string Path = path;
    public readonly string Method = method;
    public readonly Delegate Handler = handler;

    public bool Mathces(RequestContext context)
    {
        return context.Method.Equals(Method, StringComparison.OrdinalIgnoreCase) &&
            context.Path.StartsWith(Path);
    }
}