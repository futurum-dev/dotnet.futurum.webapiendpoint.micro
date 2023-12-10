using Asp.Versioning;
using Asp.Versioning.ApiExplorer;

using Futurum.WebApiEndpoint.Micro.Generator;

namespace Futurum.WebApiEndpoint.Micro.Tests;

public class WebApiOpenApiVersionConfigurationServiceTests
{
    [Fact]
    public void when_IsDeprecated()
    {
        var webApiOpenApiVersionConfigurationService = new WebApiOpenApiVersionConfigurationService(new WebApiEndpointConfiguration(WebApiEndpointVersion.Create(1.0d)));

        var apiVersionDescription = new ApiVersionDescription(new ApiVersion(1, 0), string.Empty, true, null);

        var openApiInfo = webApiOpenApiVersionConfigurationService.CreateOpenApiInfo(apiVersionDescription);

        openApiInfo.Description.Should().Be(" This API version has been deprecated.");
    }

    [Fact]
    public void when_SunsetPolicy()
    {
        var webApiOpenApiVersionConfigurationService = new WebApiOpenApiVersionConfigurationService(new WebApiEndpointConfiguration(WebApiEndpointVersion.Create(1.0d)));

        var linkHeaderValue = new LinkHeaderValue(new Uri("https://www.google.com"), "sunset")
        {
            Title = "Test",
            Type = "text/html"
        };
        var sunsetPolicy = new SunsetPolicy(new DateTime(2021, 1, 1), linkHeaderValue);

        var apiVersionDescription = new ApiVersionDescription(new ApiVersion(1, 0), string.Empty, false, sunsetPolicy);

        var openApiInfo = webApiOpenApiVersionConfigurationService.CreateOpenApiInfo(apiVersionDescription);

        openApiInfo.Description.Should().Be($" The API will be sunset on 01/01/2021.{Environment.NewLine}{Environment.NewLine}Test: https://www.google.com");
    }
}
