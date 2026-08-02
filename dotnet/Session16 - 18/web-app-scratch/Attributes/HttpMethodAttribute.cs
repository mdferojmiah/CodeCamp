namespace web_app_scratch.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class HttpMethodAttribute: Attribute
{
    public string Path  { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;

    public HttpMethodAttribute(string method, string path)
    {
        Method = method;
        Path = path;
    }
}

public class HttpGetAttribute: HttpMethodAttribute
{
    public HttpGetAttribute(string path): base("GET", path){ }
}

public class HttpPostAttribute: HttpMethodAttribute
{
    public HttpPostAttribute(string path): base("POST", path) { }
}