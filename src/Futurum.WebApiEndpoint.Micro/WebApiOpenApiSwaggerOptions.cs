using Asp.Versioning.ApiExplorer;

using Microsoft.Extensions.Options;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Futurum.WebApiEndpoint.Micro;

public class WebApiOpenApiSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;
    private readonly IWebApiOpenApiVersionConfigurationService _openApiVersionConfigurationService;

    public WebApiOpenApiSwaggerOptions(IApiVersionDescriptionProvider provider,
                                       IWebApiOpenApiVersionConfigurationService openApiVersionConfigurationService)
    {
        _provider = provider;
        _openApiVersionConfigurationService = openApiVersionConfigurationService;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var apiVersionDescription in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(apiVersionDescription.GroupName, _openApiVersionConfigurationService.CreateOpenApiInfo(apiVersionDescription));
        }
    }
}