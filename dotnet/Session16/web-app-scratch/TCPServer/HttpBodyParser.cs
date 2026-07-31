using System.Text;

namespace web_app_scratch.TCPServer;

public class HttpBodyParser
{
    public static string? Parse(byte[] rawBody)
    {
        if(rawBody.Length == 0) return null;
        
        return Encoding.UTF8.GetString(rawBody);
    }
}