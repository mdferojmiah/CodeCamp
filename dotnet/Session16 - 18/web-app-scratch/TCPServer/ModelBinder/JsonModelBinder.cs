using System.Text.Json;
using web_app_scratch.TCPServer;

namespace web_app_scratch.ModelBinder;

public class JsonModelBinder
{
    public bool CanBind(RequestContext context)
    {
        if (string.IsNullOrEmpty(context.Body))
        {
            return false;
        }

        if (!context.Headers.TryGetValue("Content-Type", out var contentType))
        {
            return true;
        }
        
        return contentType.Contains("application/json");
    }


    public object? Bind(RequestContext context, Type targetType)
    {
        try
        {
            return JsonSerializer.Deserialize(context.Body!, targetType);
        }
        catch(JsonException)
        {
            throw new HttpException(400, $"Invalid Json for type {targetType.Name}");
        }
    }
}