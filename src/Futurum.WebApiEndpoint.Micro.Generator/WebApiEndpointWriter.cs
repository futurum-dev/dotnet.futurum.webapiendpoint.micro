using Futurum.WebApiEndpoint.Micro.Generator.Core;

namespace Futurum.WebApiEndpoint.Micro.Generator;

public static class WebApiEndpointWriter
{
    public static string Write(string methodName, IEnumerable<WebApiEndpointDatum> webApiEndpointData) =>
        WrapperSourceGeneratorWriter.Write(methodName, "RegisterWebApiEndpoints",
                                           codeBuilder => Write(codeBuilder, webApiEndpointData),
                                           true);

    private static void Write(IndentedStringBuilder codeBuilder, IEnumerable<WebApiEndpointDatum> webApiEndpointData)
    {
        foreach (var webApiEndpointDatum in webApiEndpointData)
        {
            codeBuilder.AppendLine($"serviceCollection.AddSingleton(typeof(global::Futurum.WebApiEndpoint.Micro.IWebApiEndpoint), typeof({webApiEndpointDatum.ImplementationType}));");
        }
    }
}