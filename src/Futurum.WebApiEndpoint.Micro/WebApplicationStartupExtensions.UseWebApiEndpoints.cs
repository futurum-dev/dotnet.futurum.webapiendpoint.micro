using System.Reflection;

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

        foreach (var webApiEndpoint in webApiEndpoints)
        {
            var webApiVersionAttributes = webApiEndpoint.GetType().GetCustomAttributes<WebApiEndpointVersionAttribute>();

            if (!webApiVersionAttributes.Any())
            {
                CreateWebApiEndpoint(app, webApiEndpoint, configuration, configuration.DefaultWebApiEndpointVersion, null);
            }
            else
            {
                foreach (var webApiVersionAttribute in webApiVersionAttributes)
                {
                    CreateWebApiEndpoint(app, webApiEndpoint, configuration, configuration.DefaultWebApiEndpointVersion, webApiVersionAttribute);
                }
            }
        }

        return app;
    }

    private static void CreateWebApiEndpoint(WebApplication app, IWebApiEndpoint webApiEndpoint, WebApiEndpointConfiguration configuration,
                                             WebApiEndpointVersion defaultWebApiEndpointVersion, WebApiEndpointVersionAttribute? webApiVersionAttribute)
    {
        var webApiEndpointAttribute = webApiEndpoint.GetType().GetCustomAttribute<WebApiEndpointAttribute>();

        var tag = GetTag(webApiEndpointAttribute);

        var webApiEndpointVersion = GetVersion(defaultWebApiEndpointVersion, webApiVersionAttribute);

        var route = GetRoute(configuration, webApiEndpointAttribute);

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

    private static string GetRoute(WebApiEndpointConfiguration configuration, WebApiEndpointAttribute? webApiEndpointAttribute)
    {
        var prefixRoute = webApiEndpointAttribute?.PrefixRoute ?? string.Empty;

        return $"{configuration.VersionPrefix}{{version:apiVersion}}/{prefixRoute}";
    }

    private static string GetTag(WebApiEndpointAttribute? webApiEndpointAttribute)
    {
        if (webApiEndpointAttribute?.Group != null)
        {
            return webApiEndpointAttribute.Group;
        }

        if (webApiEndpointAttribute?.PrefixRoute != null)
        {
            return webApiEndpointAttribute.PrefixRoute;
        }

        return string.Empty;
    }

    private static WebApiEndpointVersion GetVersion(WebApiEndpointVersion webApiEndpointVersion, WebApiEndpointVersionAttribute? webApiVersionAttribute) =>
        webApiVersionAttribute != null
            ? new WebApiEndpointVersion(webApiVersionAttribute.MajorVersion, webApiVersionAttribute.MinorVersion)
            : webApiEndpointVersion;
}