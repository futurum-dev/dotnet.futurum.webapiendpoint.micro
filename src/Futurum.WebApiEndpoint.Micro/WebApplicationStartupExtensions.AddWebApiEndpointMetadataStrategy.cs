namespace Futurum.WebApiEndpoint.Micro;

public static partial class WebApplicationStartupExtensions
{
    /// <summary>
    /// Add a custom version of <see cref="IWebApiEndpointMetadataStrategy"/>
    /// </summary>
    public static IServiceCollection AddWebApiEndpointMetadataStrategy<TMetadataStrategy>(this IServiceCollection serviceCollection)
        where TMetadataStrategy : class, IWebApiEndpointMetadataStrategy
    {
        serviceCollection.AddSingleton<IWebApiEndpointMetadataStrategy, TMetadataStrategy>();

        return serviceCollection;
    }
}