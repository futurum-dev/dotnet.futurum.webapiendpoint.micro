using Asp.Versioning;

namespace Futurum.WebApiEndpoint.Micro;

public sealed class WebApiEndpointCreationException(WebApiEndpointVersion webApiEndpointVersion, string route, string tag, Exception innerException)
    : Exception($"Failed to create WebApiEndpoint. Version '{webApiEndpointVersion.ApiVersion}', Route '{route}' and Tag '{tag}'", innerException);

public sealed class GlobalWebApiEndpointConfigureException(Type globalWebApiEndpointType, WebApiEndpointConfiguration configuration, Exception innerException)
    : Exception($"Failed to configure GlobalWebApiEndpoint, using '{globalWebApiEndpointType.FullName}', with configuration '{configuration}'", innerException);

public sealed class WebApiVersionEndpointConfigureException(Type webApiVersionEndpointType, WebApiEndpointConfiguration configuration, Exception innerException)
    : Exception($"Failed to configure WebApiVersionEndpoint, using '{webApiVersionEndpointType.FullName}', with configuration '{configuration}'", innerException);

public abstract class WebApiEndpoint : IWebApiEndpoint
{
    public abstract void Register(IEndpointRouteBuilder builder, WebApiEndpointConfiguration configuration);

    protected virtual RouteGroupBuilder Configure(RouteGroupBuilder groupBuilder, WebApiEndpointVersion webApiEndpointVersion) =>
        groupBuilder;

    protected abstract void Build(IEndpointRouteBuilder builder);

    protected static RouteGroupBuilder CreateWebApiEndpoint(IEndpointRouteBuilder app, WebApiEndpointConfiguration configuration, WebApiEndpointVersion webApiEndpointVersion, string route, string tag)
    {
        try
        {
            var workingEndpointRouteBuilder = CreateVersionedEndpointRouteBuilder(app, configuration, webApiEndpointVersion);

            workingEndpointRouteBuilder = ConfigureWithGlobalWebApiEndpoint(app, configuration, workingEndpointRouteBuilder);

            workingEndpointRouteBuilder = ApplyVersionedRoute(workingEndpointRouteBuilder, configuration);

            workingEndpointRouteBuilder = ConfigureWithWebApiVersionEndpoint(app, configuration, webApiEndpointVersion, workingEndpointRouteBuilder);

            return ApplySpecificRouteAndTag(workingEndpointRouteBuilder, route, tag, webApiEndpointVersion);
        }
        catch (Exception exception)
        {
            throw new WebApiEndpointCreationException(webApiEndpointVersion, route, tag, exception);
        }
    }

    private static IEndpointRouteBuilder CreateVersionedEndpointRouteBuilder(IEndpointRouteBuilder app, WebApiEndpointConfiguration configuration, WebApiEndpointVersion webApiEndpointVersion)
    {
        ApiVersion apiVersion = webApiEndpointVersion;

        var formattedVersion = apiVersion.ToString(configuration.Version.Format, new ApiVersionFormatProvider());

        return app.NewVersionedApi($"{configuration.Version.Prefix}{formattedVersion}");
    }

    private static IEndpointRouteBuilder ConfigureWithGlobalWebApiEndpoint(IEndpointRouteBuilder app, WebApiEndpointConfiguration configuration, IEndpointRouteBuilder workingEndpointRouteBuilder)
    {
        var globalWebApiEndpoint = app.ServiceProvider.GetService<IGlobalWebApiEndpoint>();

        if (globalWebApiEndpoint == null) return workingEndpointRouteBuilder;

        try
        {
            return globalWebApiEndpoint.Configure(workingEndpointRouteBuilder, configuration);
        }
        catch (Exception exception)
        {
            throw new GlobalWebApiEndpointConfigureException(globalWebApiEndpoint.GetType(), configuration, exception);
        }
    }

    private static IEndpointRouteBuilder ConfigureWithWebApiVersionEndpoint(IEndpointRouteBuilder app, WebApiEndpointConfiguration configuration, WebApiEndpointVersion webApiEndpointVersion,
                                                                            IEndpointRouteBuilder workingEndpointRouteBuilder)
    {
        var webApiVersionEndpoint = app.ServiceProvider.GetKeyedService<IWebApiVersionEndpoint>(webApiEndpointVersion);

        if (webApiVersionEndpoint == null) return workingEndpointRouteBuilder;

        try
        {
            return webApiVersionEndpoint.Configure(workingEndpointRouteBuilder, configuration);
        }
        catch (Exception exception)
        {
            throw new WebApiVersionEndpointConfigureException(webApiVersionEndpoint.GetType(), configuration, exception);
        }
    }

    private static RouteGroupBuilder ApplyVersionedRoute(IEndpointRouteBuilder app, WebApiEndpointConfiguration configuration) =>
        app.MapGroup($"{configuration.Version.Prefix}{{version:apiVersion}}");

    private static RouteGroupBuilder ApplySpecificRouteAndTag(IEndpointRouteBuilder endpointRouteBuilder, string route, string tag, WebApiEndpointVersion webApiEndpointVersion)
    {
        var routeGroupBuilder = endpointRouteBuilder.MapGroup($"{route}");

        if (!string.IsNullOrEmpty(tag))
        {
            routeGroupBuilder.MapGroup(tag);
            routeGroupBuilder.WithTags(tag);
        }

        ApiVersion apiVersion = webApiEndpointVersion;

        routeGroupBuilder.HasApiVersion(apiVersion);

        return routeGroupBuilder;
    }
}
