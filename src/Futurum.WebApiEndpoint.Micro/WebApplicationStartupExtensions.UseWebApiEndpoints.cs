using Asp.Versioning;
using Asp.Versioning.Builder;

namespace Futurum.WebApiEndpoint.Micro;

public static partial class WebApplicationStartupExtensions
{
    /// <summary>
    /// Adds the WebApiEndpoints to the pipeline
    /// </summary>
    public static WebApplication UseWebApiEndpoints(this WebApplication app)
    {
        var webApiEndpoints = app.Services.GetServices<IWebApiEndpoint>();

        var configuration = app.Services.GetService<WebApiEndpointConfiguration>()!;
        
        var metadataStrategy = app.Services.GetService<IWebApiEndpointMetadataStrategy>()!;

        foreach (var webApiEndpoint in webApiEndpoints)
        {
            foreach (var webApiEndpointMetadata in metadataStrategy.Get(configuration, webApiEndpoint))
            {
                CreateWebApiEndpoint(app, webApiEndpoint, configuration, webApiEndpointMetadata.WebApiEndpointVersion, webApiEndpointMetadata.PrefixRoute, webApiEndpointMetadata.Tag);
            }
        }

        return app;
    }

    private static void CreateWebApiEndpoint(WebApplication app, IWebApiEndpoint webApiEndpoint, WebApiEndpointConfiguration configuration, WebApiEndpointVersion webApiEndpointVersion, string route,
                                             string tag)
    {
        var routeGroupBuilder = CreateRouteGroupBuilder(app, configuration, webApiEndpointVersion, route, tag);

        webApiEndpoint.Configure(routeGroupBuilder, webApiEndpointVersion);

        webApiEndpoint.Register(routeGroupBuilder);
    }

    private static RouteGroupBuilder CreateRouteGroupBuilder(WebApplication app, WebApiEndpointConfiguration configuration, WebApiEndpointVersion webApiEndpointVersion, string route, string tag)
    {
        var versionedEndpointRouteBuilder = CreateVersionedEndpointRouteBuilder(app, configuration, webApiEndpointVersion);

        return CreateRouteGroupBuilderVersioned(versionedEndpointRouteBuilder, route, tag, webApiEndpointVersion);
    }

    private static RouteGroupBuilder CreateRouteGroupBuilderVersioned(IEndpointRouteBuilder endpointRouteBuilder, string route, string tag, WebApiEndpointVersion webApiEndpointVersion)
    {
        var routeGroupBuilder = endpointRouteBuilder.MapGroup(route);

        if (!string.IsNullOrEmpty(tag))
        {
            routeGroupBuilder.MapGroup(tag);
            routeGroupBuilder.WithTags(tag);
        }

        routeGroupBuilder.HasApiVersion(webApiEndpointVersion.MajorVersion, webApiEndpointVersion.MinorVersion);

        return routeGroupBuilder;
    }

    private static IVersionedEndpointRouteBuilder CreateVersionedEndpointRouteBuilder(WebApplication app, WebApiEndpointConfiguration configuration, WebApiEndpointVersion webApiEndpointVersion)
    {
        var apiVersion = (ApiVersion)webApiEndpointVersion;

        var formattedVersion = apiVersion.ToString(configuration.VersionFormat);

        return app.NewVersionedApi($"{configuration.VersionPrefix}{formattedVersion}");
    }
}