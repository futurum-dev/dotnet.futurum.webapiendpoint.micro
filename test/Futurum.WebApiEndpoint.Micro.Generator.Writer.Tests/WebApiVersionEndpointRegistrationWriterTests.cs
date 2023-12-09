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
                    new WebApiEndpointVersionDatum(1, 0)
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
                    new(1, 0),
                    new(2, 0),
                }),
        };

        var result = WebApiVersionEndpointRegistrationWriter.Write(nameof(WebApiVersionEndpointDatum), webApiVersionEndpointData);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }
}