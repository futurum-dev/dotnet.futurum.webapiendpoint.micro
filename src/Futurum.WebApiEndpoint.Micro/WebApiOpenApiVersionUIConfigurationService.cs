using Asp.Versioning.ApiExplorer;

using Swashbuckle.AspNetCore.SwaggerUI;

namespace Futurum.WebApiEndpoint.Micro;

public interface IWebApiOpenApiVersionUIConfigurationService
{
    void CreateOpenApiInfo(SwaggerUIOptions options, ApiVersionDescription apiVersionDescription);
}

public class WebApiOpenApiVersionUIConfigurationService : IWebApiOpenApiVersionUIConfigurationService
{
    public void CreateOpenApiInfo(SwaggerUIOptions options, ApiVersionDescription apiVersionDescription)
    {
        var url = $"/swagger/{apiVersionDescription.GroupName}/swagger.json";
        var name = apiVersionDescription.GroupName.ToUpperInvariant();
        options.SwaggerEndpoint(url, name);
    }
}