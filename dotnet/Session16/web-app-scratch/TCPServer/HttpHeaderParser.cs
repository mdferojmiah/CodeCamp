using System.Text;

namespace web_app_scratch.TCPServer;

public class HttpHeaderParser
{
    public static RequestContext Parse(byte[] rawHeader)
    {
        var headerString = Encoding.UTF8.GetString(rawHeader);
        var lines = headerString.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
        var requestLine = lines[0].Split(' ');

        var context = new RequestContext
        {
            Method = requestLine[0],
            Path = requestLine[1],
            Version = requestLine.Length > 2 ? requestLine[2] : "HTTP/1.1"
        };
        
        for(int i = 1; i < lines.Length - 1; i++)
        {
            var colon = lines[i].IndexOf(':');
            var key = lines[i][..colon].Trim();
            var value = lines[i][(colon + 1)..].Trim();
            context.Headers[key] = value;
        }

        return context;
    }
}