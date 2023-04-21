using System.Collections.Generic;
using System.Threading.Tasks;

using VerifyXunit;

using Xunit;

namespace Futurum.WebApiEndpoint.Micro.Generator.Writer.Tests;

[UsesVerify]
public class WebApiEndpointWriterTests
{
    [Fact]
    public Task check()
    {
        var webApiEndpointData = new List<WebApiEndpointDatum>
        {
            new("Futurum.WebApiEndpoint.Micro.Generator.Writer.Tests.WebApiEndpoint1"),
        };

        var result = WebApiEndpointWriter.Write(nameof(WebApiEndpointDatum), webApiEndpointData);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }
}