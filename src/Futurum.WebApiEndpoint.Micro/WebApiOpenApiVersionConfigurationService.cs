using System.Text;

using Asp.Versioning;
using Asp.Versioning.ApiExplorer;

using Microsoft.OpenApi.Models;

namespace Futurum.WebApiEndpoint.Micro;

public interface IWebApiOpenApiVersionConfigurationService
{
    OpenApiInfo CreateOpenApiInfo(ApiVersionDescription apiVersionDescription);
}

public class WebApiOpenApiVersionConfigurationService(WebApiEndpointConfiguration configuration) : IWebApiOpenApiVersionConfigurationService
{
    public OpenApiInfo CreateOpenApiInfo(ApiVersionDescription apiVersionDescription)
    {
        var description = new StringBuilder();

        if (apiVersionDescription.IsDeprecated)
        {
            ConfigureDescriptionForDeprecatedOpenApi(description);
        }

        if (apiVersionDescription.SunsetPolicy != null)
        {
            ConfigureDescriptionForSunsetPolicyOpenApi(apiVersionDescription.SunsetPolicy, description);
        }

        var openApiInfo = GetVersionedOpenApiInfo(apiVersionDescription);
        openApiInfo.Description = description.ToString();

        return openApiInfo;
    }

    private OpenApiInfo GetVersionedOpenApiInfo(ApiVersionDescription apiVersionDescription)
    {
        var webApiVersion = new WebApiEndpointVersion(apiVersionDescription.ApiVersion.MajorVersion ?? int.MinValue, apiVersionDescription.ApiVersion.MinorVersion ?? int.MinValue);
        if (configuration.OpenApi.VersionedInfo.TryGetValue(webApiVersion, out var openApiInfoForVersion))
        {
            return openApiInfoForVersion;
        }

        return configuration.OpenApi.DefaultInfo;
    }

    private static void ConfigureDescriptionForDeprecatedOpenApi(StringBuilder description)
    {
        description.Append(" This API version has been deprecated.");
    }

    private static void ConfigureDescriptionForSunsetPolicyOpenApi(SunsetPolicy sunsetPolicy, StringBuilder description)
    {
        if (sunsetPolicy.Date is DateTimeOffset when)
        {
            description.Append(" The API will be sunset on ")
                       .Append(when.Date.ToShortDateString())
                       .Append('.');
        }

        if (sunsetPolicy.HasLinks)
        {
            description.AppendLine();

            foreach (var link in sunsetPolicy.Links)
            {
                LinkToDescription(description, link);
            }
        }
    }

    private static void LinkToDescription(StringBuilder description, LinkHeaderValue link)
    {
        if (link.Type == "text/html")
        {
            description.AppendLine();

            if (link.Title.HasValue)
            {
                description.Append(link.Title.Value).Append(": ");
            }

            description.Append(link.LinkTarget.OriginalString);
        }
    }
}
