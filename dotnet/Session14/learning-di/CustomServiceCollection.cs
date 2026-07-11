using System.Security.Cryptography;

namespace learning_di;

public class CustomServiceCollection
{
    private readonly List<ServiceDescriptors> _serviceDescriptors;

    public CustomServiceCollection()
    {
        _serviceDescriptors = new List<ServiceDescriptors>();
    }

    public void AddTransient<TserviceType, TimplementationType>()
    {
        var descriptor = new ServiceDescriptors(
            typeof(TserviceType),
            typeof(TimplementationType),
            ServiceLifeTime.Transient
        );
        _serviceDescriptors.Add(descriptor);
    }

    public ServiceProvider BuildServiceProvider()
    {
        return new ServiceProvider(_serviceDescriptors.AsReadOnly());
    }
}

