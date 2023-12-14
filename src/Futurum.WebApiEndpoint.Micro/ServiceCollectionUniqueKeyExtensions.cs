namespace Futurum.WebApiEndpoint.Micro;

public static class ServiceCollectionUniqueKeyExtensions
{
    public static void TryAddEquatableKeyedSingleton(this IServiceCollection collection, Type service, object? serviceKey, Type implementationType)
    {
        var descriptor = ServiceDescriptor.KeyedSingleton(service, serviceKey, implementationType);
        collection.TryAddEquatableKeyed(descriptor);
    }

    private static void TryAddEquatableKeyed(this IServiceCollection collection, ServiceDescriptor descriptor)
    {
        var count = collection.Count;
        for (var index = 0; index < count; ++index)
        {
            var serviceKey = collection[index].ServiceKey;
            if (serviceKey != null && collection[index].ServiceType == descriptor.ServiceType && serviceKey.Equals(descriptor.ServiceKey))
                return;
        }
        collection.Add(descriptor);
    }
}
