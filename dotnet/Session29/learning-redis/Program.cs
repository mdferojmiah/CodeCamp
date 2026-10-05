using learning_redis;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var connectionString = sp.GetRequiredService<IConfiguration>().GetConnectionString("Redis");
    var lazy = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(connectionString!));
    return lazy.Value;
});
builder.Services.AddHealthChecks()
    .AddCheck<RedisHealthCheak>("Redis");

builder.Services.AddControllers();

var app = builder.Build();

app.MapHealthChecks("/health");

app.UseHttpsRedirection();

app.MapControllers();

var subcriber = app.Services
    .GetRequiredService<IConnectionMultiplexer>()
    .GetSubscriber();

await subcriber.SubscribeAsync(RedisChannel.Literal("system-notifier"), (channel, message) =>
{
    Console.WriteLine($"Received Message: {message}");
});
    
app.Run();
