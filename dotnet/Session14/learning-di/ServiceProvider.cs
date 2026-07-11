namespace learning_di;

public class ServiceProvider
{
    private readonly IReadOnlyList<ServiceDescriptors> _serviceDescriptors;

    public ServiceProvider(IReadOnlyList<ServiceDescriptors> serviceDescriptors)
    {
        _serviceDescriptors = serviceDescriptors;
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
            _ => throw new NotImplementedException()
        }; 
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
}