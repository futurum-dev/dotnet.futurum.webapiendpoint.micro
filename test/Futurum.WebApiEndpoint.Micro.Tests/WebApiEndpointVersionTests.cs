using Futurum.WebApiEndpoint.Micro.Generator;

namespace Futurum.WebApiEndpoint.Micro.Tests;

public class WebApiEndpointVersionTests
{
    [Fact]
    public void WebApiEndpointNumberApiVersion_check()
    {
        // Arrange
        var version = 1.0;
        var status = "status";

        // Act
        var result = WebApiEndpointVersion.Create(version, status);

        // Assert
        result.ApiVersion.Should().BeOfType<WebApiEndpointApiVersion.WebApiEndpointNumberApiVersion>();
        var apiVersion = (WebApiEndpointApiVersion.WebApiEndpointNumberApiVersion)result.ApiVersion;
        apiVersion.Version.Should().Be(version);
        apiVersion.Status.Should().Be(status);
    }

    [Fact]
    public void WebApiEndpointStringApiVersion_check()
    {
        // Arrange
        var version = "1.0";

        // Act
        var result = WebApiEndpointVersion.Create(version);

        // Assert
        result.ApiVersion.Should().BeOfType<WebApiEndpointApiVersion.WebApiEndpointStringApiVersion>();
        var apiVersion = (WebApiEndpointApiVersion.WebApiEndpointStringApiVersion)result.ApiVersion;
        apiVersion.Version.Should().Be(version);
    }
}
