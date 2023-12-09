using System.Threading.Tasks;

using VerifyXunit;

using Xunit;

namespace Futurum.WebApiEndpoint.Micro.Generator.Writer.Tests;

[UsesVerify]
public class SourceGeneratorWriterTests
{
    [Fact]
    public Task check()
    {
        var projectName = "Futurum.WebApiEndpoint.Micro.Generator.Writer.Tests";

        var result = SourceGeneratorWriter.Write(projectName);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }
}