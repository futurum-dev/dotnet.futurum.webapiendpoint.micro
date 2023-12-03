using Futurum.WebApiEndpoint.Micro.Generator.Core;

namespace Futurum.WebApiEndpoint.Micro.Generator;

public static class SourceGeneratorWriter
{
    public static string Write(string methodName, bool skipVersion = false) =>
        WrapperSourceGeneratorWriter.Write(methodName, $"AddWebApiEndpointsFor{methodName}",
                                           Write,
                                           false,
                                           skipVersion);

    private static void Write(IndentedStringBuilder codeBuilder)
    {
        codeBuilder.AppendLine("serviceCollection.RegisterWebApiEndpoints();");
    }
}
