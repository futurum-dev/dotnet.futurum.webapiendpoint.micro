using Asp.Versioning.ApiExplorer;

namespace Futurum.WebApiEndpoint.Micro;

public static partial class WebApplicationStartupExtensions
{
    /// <summary>
    /// Register the OpenApi UI (Swagger and SwaggerUI) middleware
    /// <para><see cref="ApiVersionDescription"/>'s are configured using <see cref="IWebApiOpenApiVersionUIConfigurationService"/></para>
    /// </summary>
    public static WebApplication UseWebApiEndpointsOpenApi(this WebApplication app)
    {
        app.UseSwagger();

        app.UseSwaggerUI(options =>
        {
            var openApiVersionUiConfigurationService = app.Services.GetRequiredService<IWebApiOpenApiVersionUIConfigurationService>();

            var apiVersionDescriptions = app.DescribeApiVersions();

            foreach (var apiVersionDescription in apiVersionDescriptions)
            {
                openApiVersionUiConfigurationService.CreateOpenApiInfo(options, apiVersionDescription);
            }
        });

        return app;
    }
}