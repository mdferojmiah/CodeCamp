namespace learning_di;


public enum ServiceLifeTime
{
    Transient,
    Scoped,
    Singleton
}

public class ServiceDescriptors(
    Type serviceType,
    Type implementationType,
    ServiceLifeTime lifeTime
)
{
    public Type ServiceType { get; set; } = serviceType;
    public Type ImplementationType { get; set; } = implementationType;
    public ServiceLifeTime LifeTime { get; set; } = lifeTime;
    public object? SingletonInstance { get; set; }
    public object SingletonLock { get; set; } = new();
}