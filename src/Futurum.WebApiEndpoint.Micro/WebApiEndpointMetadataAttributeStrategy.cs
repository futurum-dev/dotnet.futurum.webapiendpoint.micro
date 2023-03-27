using System.Reflection;

namespace Futurum.WebApiEndpoint.Micro;

public class WebApiEndpointMetadataAttributeStrategy : IWebApiEndpointMetadataStrategy
{
    public IEnumerable<WebApiEndpointMetadata> Get<TWebApiEndpoint>(WebApiEndpointConfiguration configuration, TWebApiEndpoint webApiEndpoint)
        where TWebApiEndpoint : IWebApiEndpoint
    {
        var webApiEndpointType = webApiEndpoint.GetType();
        
        var webApiEndpointAttribute = webApiEndpointType.GetCustomAttribute<WebApiEndpointAttribute>();

        var webApiVersionAttributes = webApiEndpoint.GetType().GetCustomAttributes<WebApiEndpointVersionAttribute>();

        if (!webApiVersionAttributes.Any())
        {
            var tag = GetTag(webApiEndpointAttribute);

            var webApiEndpointVersion = GetVersion(configuration.DefaultWebApiEndpointVersion, null);

            var prefixRoute = GetPrefixRoute(configuration, webApiEndpointAttribute);

            yield return new WebApiEndpointMetadata(prefixRoute, tag, webApiEndpointVersion);
        }
        else
        {
            foreach (var webApiVersionAttribute in webApiVersionAttributes)
            {
                var tag = GetTag(webApiEndpointAttribute);

                var webApiEndpointVersion = GetVersion(configuration.DefaultWebApiEndpointVersion, webApiVersionAttribute);

                var prefixRoute = GetPrefixRoute(configuration, webApiEndpointAttribute);

                yield return new WebApiEndpointMetadata(prefixRoute, tag, webApiEndpointVersion);
            }
        }
    }

    private static string GetPrefixRoute(WebApiEndpointConfiguration configuration, WebApiEndpointAttribute? webApiEndpointAttribute)
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