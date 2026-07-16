namespace learning_di;

public class ServiceProvider
{
    private readonly IReadOnlyList<ServiceDescriptors> _serviceDescriptors;
    private readonly Dictionary<Type, object>? _scopedInstances;

    public ServiceProvider(IReadOnlyList<ServiceDescriptors> serviceDescriptors)
    {
        _serviceDescriptors = serviceDescriptors;
    }
    private ServiceProvider(IReadOnlyList<ServiceDescriptors> serviceDescriptors, bool isScoped)
    {
        _serviceDescriptors = serviceDescriptors;
        if(isScoped) _scopedInstances = new();
    }

    public T GetRequiredService<T>()
    {
        return (T)GetService(typeof(T));
    }

    public object GetService(Type serviceType)
    {
        var descriptor = _serviceDescriptors.FirstOrDefault(x => x.ServiceType == serviceType)
                        ?? throw new Exception($"{serviceType.Name} is not Registered!");

        return descriptor.LifeTime switch
        {
            ServiceLifeTime.Transient => CreateInstance(descriptor.ImplementationType),
            ServiceLifeTime.Singleton => CreateSingletonInstance(descriptor),
            ServiceLifeTime.Scoped => CreateScopedInstance(descriptor),
            _ => throw new NotImplementedException()
        }; 
    }

    public object CreateScopedInstance(ServiceDescriptors descriptor)
    {
        if(_scopedInstances == null)
            return new NotImplementedException("Cannot resolve scoped service from root provider");

        if (_scopedInstances.TryGetValue(descriptor.ServiceType, out var instance)) return instance;
        instance = CreateInstance(descriptor.ImplementationType);
        _scopedInstances[descriptor.ServiceType] = instance;
        return instance;
    }

    public ServiceScope CreateScope()
    {
        ServiceProvider provider = new ServiceProvider(_serviceDescriptors, true);
        return new ServiceScope(provider);
    }

    public object CreateSingletonInstance(ServiceDescriptors descriptor)
    {
        // if(descriptor.SingletonInstance == null)
        // {
        //     descriptor.SingletonInstance = CreateInstance(descriptor.ImplementationType);
        // }
        //lock is for thread safety
        lock(descriptor.SingletonLock)
        {
            descriptor.SingletonInstance ??= CreateInstance(descriptor.ImplementationType);
            return descriptor.SingletonInstance;
        }
    }

    public object CreateInstance(Type implType)
    {
        var constructor = implType.GetConstructors();
        var firstConstructor = constructor.FirstOrDefault()
                                ?? throw new Exception($"No public constructor found for {implType.Name}");
        
        var parameters = firstConstructor.GetParameters()
                        .Select(x => GetService(x.ParameterType))
                        .ToArray();
        
        if(parameters.Length > 0) return Activator.CreateInstance(implType, parameters)!;

        return Activator.CreateInstance(implType)!;
    }

    internal void DisposeScopedInstances()
    {
        if(_scopedInstances == null) return;

        foreach(var instance in _scopedInstances.Values)
        {
            if(instance is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
        _scopedInstances.Clear();
    }
}