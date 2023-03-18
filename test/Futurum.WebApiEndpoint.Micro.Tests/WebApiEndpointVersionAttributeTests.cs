using FluentAssertions;

namespace Futurum.WebApiEndpoint.Micro.Tests;

public class WebApiEndpointVersionAttributeTests
{
    [Fact]
    public void when_only_MajorVersion()
    {
        var majorVersion = 1;

        var webApiVersionAttribute = new WebApiEndpointVersionAttribute(majorVersion);

        webApiVersionAttribute.MajorVersion.Should().Be(majorVersion);
        webApiVersionAttribute.MinorVersion.Should().BeNull();
    }

    [Fact]
    public void when_MajorVersion_and_MinorVersion_is_not_null()
    {
        var majorVersion = 1;
        var minorVersion = 2;

        var webApiVersionAttribute = new WebApiEndpointVersionAttribute(majorVersion, minorVersion);

        webApiVersionAttribute.MajorVersion.Should().Be(majorVersion);
        webApiVersionAttribute.MinorVersion.Should().Be(minorVersion);
    }

    [Fact]
    public void when_MajorVersion_and_MinorVersion_is_null()
    {
        var majorVersion = 1;

        var webApiVersionAttribute = new WebApiEndpointVersionAttribute(majorVersion, null);

        webApiVersionAttribute.MajorVersion.Should().Be(majorVersion);
        webApiVersionAttribute.MinorVersion.Should().BeNull();
    }
}