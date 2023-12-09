using System.Threading.Tasks;

using VerifyXunit;

using Xunit;

namespace Futurum.WebApiEndpoint.Micro.Generator.Writer.Tests;

[UsesVerify]
public class GlobalWebApiEndpointRegistrationWriterTests
{
    [Fact]
    public Task check()
    {
        var globalWebApiEndpointDatum = new GlobalWebApiEndpointDatum("Futurum.WebApiEndpoint.Micro.Generator.Writer.Tests",
                                                               "GlobalWebApiEndpoint1",
                                                               "Futurum.WebApiEndpoint.Micro.Generator.Writer.Tests.GlobalWebApiEndpoint1");

        var result = GlobalWebApiEndpointRegistrationWriter.Write(nameof(GlobalWebApiEndpointDatum), globalWebApiEndpointDatum);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }
}
