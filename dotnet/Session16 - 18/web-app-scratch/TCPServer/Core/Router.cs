using System.Runtime.CompilerServices;
using web_app_scratch.ModelBinder;
using web_app_scratch.TCPServer;

namespace web_app_scratch.Core;

public class Router
{
    private readonly List<EndPoint> _endpoints = [];

    public EndPoint MapGet(string path, Delegate handler)
    {
        var endPoint = new EndPoint(path, "GET", handler);
        _endpoints.Add(endPoint);
        return endPoint;
    }

    public string Resolve(RequestContext context, HandlerInvoker invoker)
    {
        var endPoint = _endpoints.FirstOrDefault(ep => ep.Mathces(context));
        if(endPoint is null) return "404 not Found!";

        return invoker.MethodInvoke(endPoint.Handler.Method, endPoint.Handler.Target, context);

        // var method = endPoint.Handler.Method;
        // var args =  new object[1];
        // args[0] = context;
        // var result = method.Invoke(endPoint.Handler.Target, args);

        // return result?.ToString() ?? "";
    }
}