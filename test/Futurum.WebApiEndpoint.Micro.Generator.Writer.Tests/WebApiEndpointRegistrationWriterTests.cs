using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using VerifyXunit;

using Xunit;

namespace Futurum.WebApiEndpoint.Micro.Generator.Writer.Tests;

[UsesVerify]
public class WebApiEndpointRegistrationWriterTests
{
    [Fact]
    public Task check()
    {
        var webApiEndpointData = new List<WebApiEndpointDatum>
        {
            new("Futurum.WebApiEndpoint.Micro.Generator.Writer.Tests",
                "WebApiEndpoint1",
                "Futurum.WebApiEndpoint.Micro.Generator.Writer.Tests.WebApiEndpoint1",
                "/api",
                "group", Enumerable.Empty<WebApiEndpointVersionDatum>()),
        };

        var result = WebApiEndpointRegistrationWriter.Write(nameof(WebApiEndpointDatum), webApiEndpointData);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }
}
