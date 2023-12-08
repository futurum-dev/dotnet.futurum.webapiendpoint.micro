using Asp.Versioning;
using Asp.Versioning.ApiExplorer;

namespace Futurum.WebApiEndpoint.Micro;

public interface IWebApiVersionConfigurationService
{
    void ConfigureApiVersioning(ApiVersioningOptions options, ApiVersion defaultWebApiVersion);
    void ConfigureApiExplorer(ApiExplorerOptions options, WebApiEndpointConfiguration configuration);
}

public class WebApiVersionConfigurationService : IWebApiVersionConfigurationService
{
    public void ConfigureApiVersioning(ApiVersioningOptions options, ApiVersion defaultWebApiVersion)
    {
        options.DefaultApiVersion = defaultWebApiVersion;
        options.AssumeDefaultVersionWhenUnspecified = true;

        // reporting api versions will return the headers
        // "api-supported-versions" and "api-deprecated-versions"
        options.ReportApiVersions = true;
    }

    public void ConfigureApiExplorer(ApiExplorerOptions options, WebApiEndpointConfiguration configuration)
    {
        // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
        // note: the specified format code will format the version as "'v'major[.minor][-status]"
        options.GroupNameFormat = $"'{configuration.Version.Prefix}'{configuration.Version.Format}";

        // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
        // can also be used to control the format of the API version in route templates
        options.SubstituteApiVersionInUrl = true;
    }
}
