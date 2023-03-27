namespace Futurum.WebApiEndpoint.Micro;

public static partial class WebApplicationStartupExtensions
{
    /// <summary>
    /// Add a custom version of <see cref="IWebApiOpenApiVersionUIConfigurationService"/>
    /// </summary>
    public static IServiceCollection AddWebApiEndpointOpenApiVersionUIConfigurationService<TOpenApiVersionUIConfigurationService>(this IServiceCollection serviceCollection)
        where TOpenApiVersionUIConfigurationService : class, IWebApiOpenApiVersionUIConfigurationService
    {
        serviceCollection.AddSingleton<IWebApiOpenApiVersionUIConfigurationService, TOpenApiVersionUIConfigurationService>();

        return serviceCollection;
    }
}