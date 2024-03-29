using Futurum.WebApiEndpoint.Micro.Generator.Core;

namespace Futurum.WebApiEndpoint.Micro.Generator;

public static class WebApiVersionEndpointRegistrationWriter
{
    public static string Write(string methodName, IEnumerable<WebApiVersionEndpointDatum> webApiVersionEndpointData) =>
        WriteWrapper(methodName, "RegisterWebApiVersionEndpoints",
                     codeBuilder => Write(codeBuilder, webApiVersionEndpointData),
                     true);

    private static void Write(IndentedStringBuilder codeBuilder, IEnumerable<WebApiVersionEndpointDatum> webApiVersionEndpointData)
    {
        foreach (var webApiVersionEndpointDatum in webApiVersionEndpointData)
        {
            foreach (var webApiEndpointVersionDatum in webApiVersionEndpointDatum.Versions)
            {
                var version = string.Empty;

                if (webApiEndpointVersionDatum.ApiVersion is WebApiEndpointApiVersion.WebApiEndpointNumberApiVersion webApiEndpointNumberApiVersion)
                {
                    version = $"((global::Asp.Versioning.ApiVersion)new global::Futurum.WebApiEndpoint.Micro.WebApiEndpointVersion(new global::Futurum.WebApiEndpoint.Micro.Generator.WebApiEndpointApiVersion.WebApiEndpointNumberApiVersion({webApiEndpointNumberApiVersion.Version}d, {webApiEndpointNumberApiVersion.Status ?? "null"}))).ToString()";
                }
                else if (webApiEndpointVersionDatum.ApiVersion is WebApiEndpointApiVersion.WebApiEndpointStringApiVersion webApiEndpointStringApiVersion)
                {
                    version = $"((global::Asp.Versioning.ApiVersion)new global::Futurum.WebApiEndpoint.Micro.WebApiEndpointVersion(new global::Futurum.WebApiEndpoint.Micro.Generator.WebApiEndpointApiVersion.WebApiEndpointStringApiVersion(\"{webApiEndpointStringApiVersion.Version}\"))).ToString()";
                }

                codeBuilder.AppendLine($"global::Futurum.Microsoft.Extensions.DependencyInjection.ServiceCollectionDescriptorExtensions.TryAddEquatableKeyedSingleton(serviceCollection, typeof(global::Futurum.WebApiEndpoint.Micro.IWebApiVersionEndpoint), {version}, typeof({webApiVersionEndpointDatum.NamespaceName}.{webApiVersionEndpointDatum.ImplementationType}));");
            }
        }
    }

    private static string WriteWrapper(string className, string methodName, Action<IndentedStringBuilder> writer, bool isNotMainMethod)
    {
        var codeBuilder = new IndentedStringBuilder();
        codeBuilder
            .AppendLine("// <auto-generated />")
            .AppendLine("#nullable enable")
            .AppendLine();

        codeBuilder
            .AppendLine("namespace Microsoft.Extensions.DependencyInjection")
            .AppendLine("{")
            .IncrementIndent();

        if (!isNotMainMethod)
        {
            codeBuilder
                .AppendLine("[global::System.Diagnostics.DebuggerNonUserCodeAttribute]")
                .AppendLine("[global::System.Diagnostics.DebuggerStepThroughAttribute]");
        }

        codeBuilder
            .AppendLine($"public static partial class {className}FuturumWebApiEndpointMicroExtensions")
            .AppendLine("{")
            .IncrementIndent();

        codeBuilder
            .Append("private static global::Microsoft.Extensions.DependencyInjection.IServiceCollection")
            .Append(" ")
            .Append(methodName)
            .AppendLine("(this global::Microsoft.Extensions.DependencyInjection.IServiceCollection serviceCollection)")
            .AppendLine("{")
            .IncrementIndent();

        writer(codeBuilder);

        codeBuilder
            .AppendLine("return serviceCollection;")
            .DecrementIndent()
            .AppendLine("}") // method
            .DecrementIndent()
            .AppendLine("}") // class
            .DecrementIndent()
            .AppendLine("}"); // namespace

        return codeBuilder.ToString();
    }
}
