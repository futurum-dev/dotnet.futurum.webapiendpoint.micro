using System.Linq;
using System.Threading.Tasks;

using VerifyXunit;

using Xunit;

namespace Futurum.WebApiEndpoint.Micro.Generator.Writer.Tests;

[UsesVerify]
public class WebApiEndpointPartialClassWriterTests
{
    [Fact]
    public Task check_NoVersion()
    {
        var webApiEndpointDatum = new WebApiEndpointDatum("Futurum.WebApiEndpoint.Micro.Generator.Writer.Tests",
                                                          "WebApiEndpoint1",
                                                          "Futurum.WebApiEndpoint.Micro.Generator.Writer.Tests.WebApiEndpoint1",
                                                          "/api",
                                                          "group",
                                                          Enumerable.Empty<WebApiEndpointVersionDatum>());

        var result = WebApiEndpointPartialClassWriter.Write(webApiEndpointDatum);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }

    [Fact]
    public Task check_SingleVersion()
    {
        var webApiEndpointDatum = new WebApiEndpointDatum("Futurum.WebApiEndpoint.Micro.Generator.Writer.Tests",
                                                          "WebApiEndpoint1",
                                                          "Futurum.WebApiEndpoint.Micro.Generator.Writer.Tests.WebApiEndpoint1",
                                                          "/api",
                                                          "group",
                                                          new []{new WebApiEndpointVersionDatum(new WebApiEndpointApiVersion.WebApiEndpointNumberApiVersion(1.0d))});

        var result = WebApiEndpointPartialClassWriter.Write(webApiEndpointDatum);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }

    [Fact]
    public Task check_MultipleVersion()
    {
        var webApiEndpointDatum = new WebApiEndpointDatum("Futurum.WebApiEndpoint.Micro.Generator.Writer.Tests",
                                                          "WebApiEndpoint1",
                                                          "Futurum.WebApiEndpoint.Micro.Generator.Writer.Tests.WebApiEndpoint1",
                                                          "/api",
                                                          "group",
                                                          Enumerable.Range(0, 5).Select(i => new WebApiEndpointVersionDatum(new WebApiEndpointApiVersion.WebApiEndpointNumberApiVersion(i))));

        var result = WebApiEndpointPartialClassWriter.Write(webApiEndpointDatum);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }
}
