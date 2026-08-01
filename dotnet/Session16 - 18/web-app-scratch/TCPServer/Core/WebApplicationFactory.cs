using web_app_scratch.DI;

namespace web_app_scratch.Core;

internal class WebApplicationBuilder
{
    public CustomServiceCollection Services { get; } = new();
    public MiniWebApplication Build()
    {
        var provider = Services.BuildServiceProvider();
        return new MiniWebApplication(provider);
    }
}

internal class WebApplicationFactory()
{
    public static WebApplicationBuilder CreateBuilder()
    {
        return new WebApplicationBuilder();
    }
}