namespace Futurum.WebApiEndpoint.Micro;

public static partial class WebApplicationStartupExtensions
{
    /// <summary>
    /// Add a custom version of <see cref="IWebApiOpenApiVersionConfigurationService"/>
    /// </summary>
    public static IServiceCollection AddWebApiEndpointOpenApiVersionConfigurationService<TOpenApiVersionConfigurationService>(this IServiceCollection serviceCollection)
        where TOpenApiVersionConfigurationService : class, IWebApiOpenApiVersionConfigurationService
    {
        serviceCollection.AddSingleton<IWebApiOpenApiVersionConfigurationService, TOpenApiVersionConfigurationService>();

        return serviceCollection;
    }
}