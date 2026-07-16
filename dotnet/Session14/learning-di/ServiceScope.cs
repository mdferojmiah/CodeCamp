namespace learning_di;

public interface IServiceScope: IDisposable
{
    ServiceProvider ServiceProvider { get; }
}

public class ServiceScope : IServiceScope
{
    public ServiceProvider _serviceProvider;

    public ServiceProvider ServiceProvider => _serviceProvider;

    public ServiceScope(ServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Dispose()
    {
        _serviceProvider.DisposeScopedInstances();
    }
}