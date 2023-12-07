using Asp.Versioning;

namespace Futurum.WebApiEndpoint.Micro;

public abstract class WebApiEndpoint : IWebApiEndpoint
{
    public abstract void Register(IEndpointRouteBuilder builder, WebApiEndpointConfiguration configuration);

    protected virtual RouteGroupBuilder Configure(RouteGroupBuilder groupBuilder, WebApiEndpointVersion webApiEndpointVersion)
    {
        return groupBuilder;
    }

    protected abstract void Build(IEndpointRouteBuilder builder);

    protected static RouteGroupBuilder CreateWebApiEndpoint(IEndpointRouteBuilder app, WebApiEndpointConfiguration configuration, WebApiEndpointVersion webApiEndpointVersion, string route,
                                                            string tag) =>
        CreateRouteGroupBuilder(app, configuration, webApiEndpointVersion, route, tag);

    private static RouteGroupBuilder CreateRouteGroupBuilder(IEndpointRouteBuilder app, WebApiEndpointConfiguration configuration, WebApiEndpointVersion webApiEndpointVersion, string route,
                                                             string tag)
    {
        var versionedEndpointRouteBuilder = CreateVersionedEndpointRouteBuilder(app, configuration, webApiEndpointVersion);

        if (TryGetRequiredKeyedService() is IWebApiVersionEndpoint webApiVersionEndpoint)
        {
            versionedEndpointRouteBuilder = webApiVersionEndpoint.Configure(versionedEndpointRouteBuilder, configuration);
        }

        return CreateRouteGroupBuilderVersioned(versionedEndpointRouteBuilder, configuration, route, tag, webApiEndpointVersion);

        object? TryGetRequiredKeyedService()
        {
            try
            {
                return app.ServiceProvider.GetRequiredKeyedService(typeof(IWebApiVersionEndpoint), webApiEndpointVersion);
            }
            catch (Exception)
            {
                // Can't find a way to check if the service exists without throwing an exception
                return null;
            }
        }
    }

    private static RouteGroupBuilder CreateRouteGroupBuilderVersioned(IEndpointRouteBuilder endpointRouteBuilder, WebApiEndpointConfiguration configuration, string route, string tag,
                                                                      WebApiEndpointVersion webApiEndpointVersion)
    {
        var routeGroupBuilder = endpointRouteBuilder.MapGroup($"{configuration.GlobalRoutePrefix}/{configuration.VersionPrefix}{{version:apiVersion}}/{route}");

        if (!string.IsNullOrEmpty(tag))
        {
            routeGroupBuilder.MapGroup(tag);
            routeGroupBuilder.WithTags(tag);
        }

        routeGroupBuilder.HasApiVersion(webApiEndpointVersion.MajorVersion, webApiEndpointVersion.MinorVersion);

        return routeGroupBuilder;
    }

    private static IEndpointRouteBuilder CreateVersionedEndpointRouteBuilder(IEndpointRouteBuilder app, WebApiEndpointConfiguration configuration, WebApiEndpointVersion webApiEndpointVersion)
    {
        var apiVersion = (ApiVersion)webApiEndpointVersion;

        var formattedVersion = apiVersion.ToString(configuration.VersionFormat);

        return app.NewVersionedApi($"{configuration.VersionPrefix}{formattedVersion}");
    }
}
