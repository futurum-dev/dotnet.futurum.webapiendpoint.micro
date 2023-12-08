using Asp.Versioning.ApiExplorer;

using Microsoft.Extensions.Options;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Futurum.WebApiEndpoint.Micro;

public class WebApiOpenApiSwaggerOptions(
    IApiVersionDescriptionProvider provider,
    IWebApiOpenApiVersionConfigurationService openApiVersionConfigurationService)
    : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var apiVersionDescription in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(apiVersionDescription.GroupName, openApiVersionConfigurationService.CreateOpenApiInfo(apiVersionDescription));
        }
    }
}
