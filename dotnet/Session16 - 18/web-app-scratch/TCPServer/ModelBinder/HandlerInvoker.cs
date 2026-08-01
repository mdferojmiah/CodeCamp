using System.Reflection;
using web_app_scratch.DI;
using web_app_scratch.TCPServer;

namespace web_app_scratch.ModelBinder;

public class HandlerInvoker(ServiceProvider Services)
{
    private readonly JsonModelBinder _modelBinder = new();
    public string MethodInvoke(MethodInfo method, object? target, RequestContext context)
    {
        var parameters = method.GetParameters(); //createUserRequest, something
        var args =  new object?[parameters.Length];

        for(int i = 0; i < parameters.Length; i++)
        {
            var parameter = parameters[i];
            if(parameter.ParameterType == typeof(RequestContext))
            {
                args[i] = context;
                continue;
            }

            try
            {
                var service = Services.GetService(parameter.ParameterType);
                if(service != null)
                {
                    args[i] = service;
                    continue;
                }
            }catch
            {
                Console.WriteLine("Parameter doesn't exist in service provider");
            }

            if (_modelBinder.CanBind(context))
            {
                args[i] = _modelBinder.Bind(context, parameter.ParameterType);
                continue;
            }

            args[i] = null;
        }

        var result = method.Invoke(target, args);

        return result?.ToString() ?? "";
    }
}