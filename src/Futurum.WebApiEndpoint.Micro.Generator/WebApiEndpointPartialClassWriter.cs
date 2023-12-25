using Futurum.WebApiEndpoint.Micro.Generator.Core;

namespace Futurum.WebApiEndpoint.Micro.Generator;

public static class WebApiEndpointPartialClassWriter
{
    public static string Write(WebApiEndpointDatum webApiEndpointDatum) =>
        WriteWrapper(webApiEndpointDatum.NamespaceName, webApiEndpointDatum.ClassName,
                     codeBuilder => Write(codeBuilder, webApiEndpointDatum),
                     true);

    private static void Write(IndentedStringBuilder codeBuilder, WebApiEndpointDatum webApiEndpointDatum)
    {
        codeBuilder.AppendLine(
            "public override void Register(global::Microsoft.AspNetCore.Routing.IEndpointRouteBuilder builder, global::Futurum.WebApiEndpoint.Micro.WebApiEndpointConfiguration configuration)");
        codeBuilder.AppendLine("{");
        codeBuilder.IncrementIndent();

        if (!webApiEndpointDatum.Versions.Any())
        {
            codeBuilder.AppendLine("RegisterNoVersion(builder, configuration);");
            codeBuilder.AppendLine();
        }
        else
        {
            foreach (var webApiEndpointVersionDatum in webApiEndpointDatum.Versions)
            {
                codeBuilder.AppendLine($"RegisterVersion{SanitiseWebApiEndpointApiVersion(webApiEndpointVersionDatum.ApiVersion)}(builder, configuration);");
                codeBuilder.AppendLine();
            }
        }

        codeBuilder.AppendLine();
        codeBuilder.DecrementIndent();
        codeBuilder.AppendLine("}");
        codeBuilder.AppendLine();

        if (!webApiEndpointDatum.Versions.Any())
        {
            WriteNoVersion(codeBuilder, webApiEndpointDatum);
        }
        else
        {
            foreach (var webApiEndpointVersionDatum in webApiEndpointDatum.Versions)
            {
                WriteVersion(codeBuilder, webApiEndpointDatum, webApiEndpointVersionDatum);
                codeBuilder.AppendLine();
            }
        }
    }

    private static void WriteNoVersion(IndentedStringBuilder codeBuilder, WebApiEndpointDatum webApiEndpointDatum)
    {
        codeBuilder.AppendLine(
            $"private void RegisterNoVersion(global::Microsoft.AspNetCore.Routing.IEndpointRouteBuilder builder, global::Futurum.WebApiEndpoint.Micro.WebApiEndpointConfiguration configuration)");
        codeBuilder.AppendLine("{");
        codeBuilder.IncrementIndent();

        codeBuilder.AppendLine($"var webApiEndpointVersion = configuration.DefaultApiVersion;");

        codeBuilder.AppendLine();
        var tag = !string.IsNullOrEmpty(webApiEndpointDatum.Tag) ? webApiEndpointDatum.Tag : webApiEndpointDatum.Prefix;
        codeBuilder.AppendLine($"var webapiEndpointRouteBuilder = CreateWebApiEndpoint(builder, configuration, webApiEndpointVersion, \"{webApiEndpointDatum.Prefix}\", \"{tag}\");");
        codeBuilder.AppendLine();
        codeBuilder.AppendLine("webapiEndpointRouteBuilder = Configure(webapiEndpointRouteBuilder, webApiEndpointVersion);");
        codeBuilder.AppendLine();
        codeBuilder.AppendLine("Build(webapiEndpointRouteBuilder);");
        codeBuilder.AppendLine();
        codeBuilder.DecrementIndent();
        codeBuilder.AppendLine("}");
    }

    private static string SanitiseWebApiEndpointApiVersion(WebApiEndpointApiVersion apiVersion)
    {
        var version = apiVersion switch
        {
            WebApiEndpointApiVersion.WebApiEndpointNumberApiVersion numberApiVersion => string.IsNullOrEmpty(numberApiVersion.Status)
                ? $"{numberApiVersion.Version}"
                : $"{numberApiVersion.Version}_{numberApiVersion.Status}",
            WebApiEndpointApiVersion.WebApiEndpointStringApiVersion stringApiVersion => stringApiVersion.Version,
            _                                                                        => string.Empty
        };

        return version.Replace(".", "_")
                      .Replace("-", "_")
                      .Replace("(", "_")
                      .Replace(")", "_")
                      .Replace("[", "_")
                      .Replace("]", "_")
                      .Replace("{", "_")
                      .Replace("}", "_")
                      .Replace("!", "_")
                      .Replace("@", "_")
                      .Replace("£", "_")
                      .Replace("$", "_")
                      .Replace("%", "_")
                      .Replace("^", "_")
                      .Replace("&", "_")
                      .Replace("*", "_")
                      .Replace("+", "_")
                      .Replace("=", "_")
                      .Replace("~", "_")
                      .Replace("?", "_")
                      .Replace("/", "_")
                      .Replace(" ", "_");
    }

    private static void WriteVersion(IndentedStringBuilder codeBuilder, WebApiEndpointDatum webApiEndpointDatum, WebApiEndpointVersionDatum webApiEndpointVersionDatum)
    {
        codeBuilder.AppendLine(
            $"private void RegisterVersion{SanitiseWebApiEndpointApiVersion(webApiEndpointVersionDatum.ApiVersion)}(global::Microsoft.AspNetCore.Routing.IEndpointRouteBuilder builder, global::Futurum.WebApiEndpoint.Micro.WebApiEndpointConfiguration configuration)");
        codeBuilder.AppendLine("{");
        codeBuilder.IncrementIndent();


        if (webApiEndpointVersionDatum.ApiVersion is WebApiEndpointApiVersion.WebApiEndpointNumberApiVersion numberApiVersion)
        {
            var status = numberApiVersion.Status != null
                ? $"\"{numberApiVersion.Status}\""
                : "null";
            codeBuilder.AppendLine(
                $"var webApiEndpointVersion = new global::Futurum.WebApiEndpoint.Micro.WebApiEndpointVersion(new global::Futurum.WebApiEndpoint.Micro.Generator.WebApiEndpointApiVersion.WebApiEndpointNumberApiVersion({numberApiVersion.Version}d, {status}));");
        }
        else if(webApiEndpointVersionDatum.ApiVersion is WebApiEndpointApiVersion.WebApiEndpointStringApiVersion stringApiVersion)
        {
            codeBuilder.AppendLine($"var webApiEndpointVersion = new global::Futurum.WebApiEndpoint.Micro.WebApiEndpointVersion(new global::Futurum.WebApiEndpoint.Micro.Generator.WebApiEndpointApiVersion.WebApiEndpointStringApiVersion(\"{stringApiVersion.Version}\"));");
        }

        codeBuilder.AppendLine();
        var tag = !string.IsNullOrEmpty(webApiEndpointDatum.Tag) ? webApiEndpointDatum.Tag : webApiEndpointDatum.Prefix;
        codeBuilder.AppendLine($"var webapiEndpointRouteBuilder = CreateWebApiEndpoint(builder, configuration, webApiEndpointVersion, \"{webApiEndpointDatum.Prefix}\", \"{tag}\");");
        codeBuilder.AppendLine();
        codeBuilder.AppendLine("webapiEndpointRouteBuilder = Configure(webapiEndpointRouteBuilder, webApiEndpointVersion);");
        codeBuilder.AppendLine();
        codeBuilder.AppendLine("Build(webapiEndpointRouteBuilder);");
        codeBuilder.AppendLine();
        codeBuilder.DecrementIndent();
        codeBuilder.AppendLine("}");
    }

    private static string WriteWrapper(string namespaceName, string className, Action<IndentedStringBuilder> writer, bool isNotMainMethod, bool skipVersion = false)
    {
        var codeBuilder = new IndentedStringBuilder();
        codeBuilder
            .AppendLine("// <auto-generated />")
            .AppendLine("#nullable enable")
            .AppendLine();

        codeBuilder
            .AppendLine($"namespace {namespaceName}")
            .AppendLine("{")
            .IncrementIndent();

        codeBuilder
            .AppendLine($"public partial class {className} : global::Futurum.WebApiEndpoint.Micro.WebApiEndpoint")
            .AppendLine("{")
            .IncrementIndent();

        writer(codeBuilder);

        codeBuilder
            .DecrementIndent()
            .AppendLine("}") // class
            .DecrementIndent()
            .AppendLine("}"); // namespace

        return codeBuilder.ToString();
    }
}
