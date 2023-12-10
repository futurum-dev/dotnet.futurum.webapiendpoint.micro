using System.Collections.Generic;
using System.Threading.Tasks;

using VerifyXunit;

using Xunit;

namespace Futurum.WebApiEndpoint.Micro.Generator.Writer.Tests;

[UsesVerify]
public class WebApiVersionEndpointRegistrationWriterTests
{
    [Fact]
    public Task check_single_version()
    {
        var webApiVersionEndpointData = new List<WebApiVersionEndpointDatum>
        {
            new("Futurum.WebApiEndpoint.Micro.Generator.Writer.Tests",
                "WebApiEndpoint1",
                "Futurum.WebApiEndpoint.Micro.Generator.Writer.Tests.WebApiEndpoint1",
                new List<WebApiEndpointVersionDatum>
                {
                    new(new WebApiEndpointApiVersion.WebApiEndpointNumberApiVersion(1.0d))
                }),
        };

        var result = WebApiVersionEndpointRegistrationWriter.Write(nameof(WebApiVersionEndpointDatum), webApiVersionEndpointData);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }

    [Fact]
    public Task check_multiple_version()
    {
        var webApiVersionEndpointData = new List<WebApiVersionEndpointDatum>
        {
            new("Futurum.WebApiEndpoint.Micro.Generator.Writer.Tests",
                "WebApiVersionEndpoint1",
                "Futurum.WebApiEndpoint.Micro.Generator.Writer.Tests.WebApiVersionEndpoint1",
                new List<WebApiEndpointVersionDatum>
                {
                    new(new WebApiEndpointApiVersion.WebApiEndpointNumberApiVersion(1.0d)),
                    new(new WebApiEndpointApiVersion.WebApiEndpointNumberApiVersion(2.0d)),
                }),
        };

        var result = WebApiVersionEndpointRegistrationWriter.Write(nameof(WebApiVersionEndpointDatum), webApiVersionEndpointData);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }
}
