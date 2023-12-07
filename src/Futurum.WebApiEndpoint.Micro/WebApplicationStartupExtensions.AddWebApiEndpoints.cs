using Asp.Versioning;

using Microsoft.Extensions.Options;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Futurum.WebApiEndpoint.Micro;

public static partial class WebApplicationStartupExtensions
{
    /// <summary>
    /// Adds services for WebApiEndpoints
    /// </summary>
    public static IServiceCollection AddWebApiEndpoints(this IServiceCollection serviceCollection, WebApiEndpointConfiguration configuration) =>
        serviceCollection.AddWebApiEndpoints<WebApiVersionConfigurationService>(configuration);

    /// <summary>
    /// Adds services for WebApiEndpoints
    /// <para>Allowing you to provide your own <see cref="IWebApiVersionConfigurationService"/></para>
    /// </summary>
    public static IServiceCollection AddWebApiEndpoints<TVersionConfigurationService>(this IServiceCollection serviceCollection, WebApiEndpointConfiguration configuration)
        where TVersionConfigurationService : class, IWebApiVersionConfigurationService, new()
    {
        serviceCollection.AddSingleton<IExceptionToProblemDetailsMapperService>(ExceptionToProblemDetailsMapperService.Instance);

        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddSwaggerGen();

        serviceCollection.AddSingleton(configuration);

        var webApiVersionConfigurationService = new TVersionConfigurationService();

        serviceCollection.AddApiVersioning(webApiVersionConfigurationService, configuration, configuration.DefaultWebApiEndpointVersion);

        return serviceCollection;
    }

    private static IServiceCollection AddApiVersioning(this IServiceCollection serviceCollection, IWebApiVersionConfigurationService webApiVersionConfigurationService,
                                                       WebApiEndpointConfiguration configuration, ApiVersion defaultWebApiVersion)
    {
        serviceCollection.AddSingleton<IConfigureOptions<SwaggerGenOptions>, WebApiOpenApiSwaggerOptions>();

        serviceCollection.AddWebApiEndpointOpenApiVersionConfigurationService<WebApiOpenApiVersionConfigurationService>();
        serviceCollection.AddWebApiEndpointOpenApiVersionUIConfigurationService<WebApiOpenApiVersionUIConfigurationService>();

        serviceCollection.AddApiVersioning(options => webApiVersionConfigurationService.ConfigureApiVersioning(options, defaultWebApiVersion))
                         .AddApiExplorer(options => webApiVersionConfigurationService.ConfigureApiExplorer(options, configuration))
                         .EnableApiVersionBinding();

        return serviceCollection;
    }
}
