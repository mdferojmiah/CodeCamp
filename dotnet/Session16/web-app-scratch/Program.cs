using web_app_scratch.Core;
using web_app_scratch.TCPServer;

var builder = WebApplicationFactory.CreateBuilder();
builder.Services.AddSingleton<ITest, Test>();


var app = builder.Build();

//minimal apis
app.MapGet("/test", (RequestContext context) =>
{
    var test = app.Services.GetRequiredService<ITest>();
    test.Print();
    return "OK";
});

await app.RunAsync();

public interface ITest
{
    void Print();
}

public class Test : ITest
{
    public void Print()
    {
        Console.WriteLine($"Random Test Service!");
    }
}