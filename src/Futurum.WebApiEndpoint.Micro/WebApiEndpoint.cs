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
        var workingEndpointRouteBuilder = CreateVersionedEndpointRouteBuilder(app, configuration, webApiEndpointVersion);

        var globalWebApiEndpoint = app.ServiceProvider.GetService<IGlobalWebApiEndpoint>();
        if (globalWebApiEndpoint != null)
        {
            workingEndpointRouteBuilder = globalWebApiEndpoint.Configure(workingEndpointRouteBuilder, configuration);
        }

        workingEndpointRouteBuilder = ApplyVersionedEndpointRouteBuilder(workingEndpointRouteBuilder, configuration);

        if (TryGetRequiredKeyedService() is IWebApiVersionEndpoint webApiVersionEndpoint)
        {
            workingEndpointRouteBuilder = webApiVersionEndpoint.Configure(workingEndpointRouteBuilder, configuration);
        }

        return CreateRouteGroupBuilderVersioned(workingEndpointRouteBuilder, configuration, route, tag, webApiEndpointVersion);

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
        var routeGroupBuilder = endpointRouteBuilder.MapGroup($"{route}");

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

    private static IEndpointRouteBuilder ApplyVersionedEndpointRouteBuilder(IEndpointRouteBuilder app, WebApiEndpointConfiguration configuration)
    {
        return app.MapGroup($"{configuration.VersionPrefix}{{version:apiVersion}}");
    }
}
