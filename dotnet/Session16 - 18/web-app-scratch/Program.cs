using web_app_scratch.Core;
using web_app_scratch.TCPServer;

var builder = WebApplicationFactory.CreateBuilder();
builder.Services.AddSingleton<ITest, Test>();


var app = builder.Build();

app.Use(async (ctx, next) =>
{
    Console.WriteLine($"{ctx.Method} {ctx.Path}");
    await next(ctx);
    Console.WriteLine($"=> {ctx.Response}");
});

//minimal apis
app.MapGet("/test", (RequestContext context) =>
{
    var test = app.Services.GetRequiredService<ITest>();
    test.Print();
    return "OK";
});

app.MapGet("/users", (CreateUserRequest request, ITest test) =>
{
    //var test = app.Services.GetRequiredService<ITest>();
    test.Print();
    Console.WriteLine($"Name: {request.Name} | Email: {request.Email} | Age: {request.Age}");
    return "OK";
});

await app.RunAsync(5005);

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

public record CreateUserRequest(string Name, string Email, int Age);