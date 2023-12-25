using System.Text;

using Asp.Versioning;
using Asp.Versioning.ApiExplorer;

using Microsoft.OpenApi.Interfaces;
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
        var openApiInfo = GetVersionedOpenApiInfo(apiVersionDescription);

        if (apiVersionDescription.IsDeprecated)
        {
            openApiInfo.Description += GetDeprecatedOpenApiDescription();
        }

        if (apiVersionDescription.SunsetPolicy != null)
        {
            openApiInfo.Description += GetSunsetPolicyOpenApiDescription(apiVersionDescription.SunsetPolicy);
        }

        return openApiInfo;
    }

    private OpenApiInfo GetVersionedOpenApiInfo(ApiVersionDescription apiVersionDescription)
    {
        if (configuration.OpenApi.VersionedOverrideInfo.TryGetValue(apiVersionDescription.ApiVersion, out var openApiInfoForVersion))
        {
            return TransformToOpenApiInfo(configuration.OpenApi.DefaultInfo, openApiInfoForVersion, apiVersionDescription.ApiVersion);
        }

        return TransformToOpenApiInfo(configuration.OpenApi.DefaultInfo, configuration.OpenApi.DefaultInfo, apiVersionDescription.ApiVersion);

        OpenApiInfo TransformToOpenApiInfo(WebApiEndpointOpenApiInfo defaultWebApiEndpointOpenApiInfo, WebApiEndpointOpenApiInfo? versionedWebApiEndpointOpenApiInfo, ApiVersion apiVersion)
        {
            var title = versionedWebApiEndpointOpenApiInfo != null
                ? versionedWebApiEndpointOpenApiInfo.Title
                : defaultWebApiEndpointOpenApiInfo.Title;

            var description = versionedWebApiEndpointOpenApiInfo != null
                ? versionedWebApiEndpointOpenApiInfo.Description
                : defaultWebApiEndpointOpenApiInfo.Description ?? string.Empty;

            var version = $"{configuration.Version.Prefix}{apiVersion.ToString(configuration.Version.Format, new ApiVersionFormatProvider())}";

            var termsOfService = versionedWebApiEndpointOpenApiInfo is { TermsOfService: not null }
                ? versionedWebApiEndpointOpenApiInfo.TermsOfService
                : defaultWebApiEndpointOpenApiInfo.TermsOfService ?? null;

            var contact = versionedWebApiEndpointOpenApiInfo is { Contact: not null }
                ? versionedWebApiEndpointOpenApiInfo.Contact
                : defaultWebApiEndpointOpenApiInfo.Contact ?? null;

            var license = versionedWebApiEndpointOpenApiInfo is { License: not null }
                ? versionedWebApiEndpointOpenApiInfo.License
                : defaultWebApiEndpointOpenApiInfo.License ?? null;

            var extensions = GetExtensions(defaultWebApiEndpointOpenApiInfo, versionedWebApiEndpointOpenApiInfo);

            return new()
            {
                Title = title,
                Description = description,
                Version = version,
                TermsOfService = termsOfService,
                Contact = contact,
                License = license,
                Extensions = extensions
            };
        }

        IDictionary<string, IOpenApiExtension> GetExtensions(WebApiEndpointOpenApiInfo defaultWebApiEndpointOpenApiInfo, WebApiEndpointOpenApiInfo? versionedWebApiEndpointOpenApiInfo)
        {
            if (versionedWebApiEndpointOpenApiInfo == null)
            {
                return defaultWebApiEndpointOpenApiInfo.Extensions;
            }

            var extensions = defaultWebApiEndpointOpenApiInfo.Extensions
                                                             .ToDictionary(kvp => kvp.Key,
                                                                           kvp => kvp.Value);

            foreach (var kvp in versionedWebApiEndpointOpenApiInfo.Extensions)
            {
                extensions[kvp.Key] = kvp.Value;
            }

            return extensions;
        }
    }

    private static string GetDeprecatedOpenApiDescription() =>
        " This API version has been deprecated.";

    private static string GetSunsetPolicyOpenApiDescription(SunsetPolicy sunsetPolicy)
    {
        var stringBuilder = new StringBuilder();

        if (sunsetPolicy.Date is { } when)
        {
            stringBuilder.Append(" The API will be sunset on ")
                         .Append(when.Date.ToShortDateString())
                         .Append('.');
        }

        if (sunsetPolicy.HasLinks)
        {
            stringBuilder.AppendLine();

            foreach (var link in sunsetPolicy.Links)
            {
                LinkToDescription(stringBuilder, link);
            }
        }

        return stringBuilder.ToString();

        static void LinkToDescription(StringBuilder description, LinkHeaderValue link)
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
}
