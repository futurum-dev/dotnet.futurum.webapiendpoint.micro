using System.Collections.Generic;
using System.Threading.Tasks;

using VerifyXunit;

using Xunit;

namespace Futurum.WebApiEndpoint.Micro.Generator.Writer.Tests;

[UsesVerify]
public class FluentValidatorWriterTests
{
    [Fact]
    public Task check()
    {
        var fluentValidatorData = new List<FluentValidatorDatum>
        {
            new("Futurum.WebApiEndpoint.Micro.Generator.Writer.Tests.IService",
                "Futurum.WebApiEndpoint.Micro.Generator.Writer.Tests.Service")
        };

        var result = FluentValidatorWriter.Write(nameof(FluentValidatorWriterTests), fluentValidatorData);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }
}