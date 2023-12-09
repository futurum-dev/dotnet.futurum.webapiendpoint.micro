using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Futurum.WebApiEndpoint.Micro.Tests;

public class WebApiResultsExtensionsToOkTests
{
    private const string REQUEST_PATH = "/RequestPath";

    private const string VALUE = "Value";

    [Fact]
    public void ToOk_without_Value()
    {
        var httpContext = CreateHttpContext() as HttpContext;
        var results = ToOk(httpContext);

        results.Should().BeOfType<Ok>();
    }

    [Fact]
    public void ToOk_with_Value()
    {
        var value = VALUE;

        var results = value.ToOk();

        results.Should().BeOfType<Ok<string>>();
        results.Value.Should().Be(VALUE);
    }

    [Fact]
    public void ToOk_with_Value_and_HttpContext()
    {
        var value = VALUE;

        var results = ToOk(CreateHttpContext(), value);

        results.Should().BeOfType<Ok<string>>();
        results.Value.Should().Be(VALUE);
    }

    private static DefaultHttpContext CreateHttpContext() =>
        new() { Request = { Path = REQUEST_PATH } };
}
