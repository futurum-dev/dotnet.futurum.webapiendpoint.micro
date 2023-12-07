using System.Text;
using System.Text.RegularExpressions;

using Futurum.WebApiEndpoint.Micro.Generator.Core;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Futurum.WebApiEndpoint.Micro.Generator;

[Generator]
public class SourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var assemblyName = context.CompilationProvider
                                  .Select(static (c, _) => c.AssemblyName);

        Generator(context, assemblyName);

        WebApiEndpoint(context, assemblyName);

        WebApiVersionEndpoint(context, assemblyName);

        GlobalWebApiEndpoint(context, assemblyName);
    }

    private static void Generator(IncrementalGeneratorInitializationContext context, IncrementalValueProvider<string?> assemblyName)
    {
        context.RegisterSourceOutput(assemblyName,
                                     static (productionContext, assemblyName) => ExecuteGeneration(productionContext, assemblyName));

        static void ExecuteGeneration(SourceProductionContext context, string assemblyName)
        {
            var methodName = Regex.Replace(assemblyName, "\\W", "");

            var codeBuilder = SourceGeneratorWriter.Write(methodName);

            context.AddSource("Generator.g.cs", SourceText.From(codeBuilder.ToString(), Encoding.UTF8));
        }
    }

    private static void WebApiEndpoint(IncrementalGeneratorInitializationContext context, IncrementalValueProvider<string?> assemblyName)
    {
        var webApiEndpointData = context.SyntaxProvider
                                        .CreateSyntaxProvider(WebApiEndpointSourceGenerator.SemanticPredicate, WebApiEndpointSourceGenerator.SemanticTransform)
                                        .Where(node => node is not null);

        var webApiEndpointDiagnostics = webApiEndpointData
                                        .Select(static (item, _) => item.Diagnostics)
                                        .Where(static item => item.Count > 0);

        context.RegisterSourceOutput(webApiEndpointDiagnostics, ReportDiagnostic);

        context.RegisterSourceOutput(webApiEndpointData.Collect().Combine(assemblyName),
                                     static (productionContext, source) => WebApiEndpointSourceGenerator.ExecuteGeneration(productionContext, source.Left, source.Right));
    }

    private static void WebApiVersionEndpoint(IncrementalGeneratorInitializationContext context, IncrementalValueProvider<string?> assemblyName)
    {
        var webApiVersionEndpointData = context.SyntaxProvider
                                        .CreateSyntaxProvider(WebApiVersionEndpointSourceGenerator.SemanticPredicate, WebApiVersionEndpointSourceGenerator.SemanticTransform)
                                        .Where(node => node is not null);

        var webApiVersionEndpointDiagnostics = webApiVersionEndpointData
                                        .Select(static (item, _) => item.Diagnostics)
                                        .Where(static item => item.Count > 0);

        context.RegisterSourceOutput(webApiVersionEndpointDiagnostics, ReportDiagnostic);

        context.RegisterSourceOutput(webApiVersionEndpointData.Collect().Combine(assemblyName),
                                     static (productionContext, source) => WebApiVersionEndpointSourceGenerator.ExecuteGeneration(productionContext, source.Left, source.Right));
    }

    private static void GlobalWebApiEndpoint(IncrementalGeneratorInitializationContext context, IncrementalValueProvider<string?> assemblyName)
    {
        var globalWebApiEndpointData = context.SyntaxProvider
                                        .CreateSyntaxProvider(GlobalWebApiEndpointSourceGenerator.SemanticPredicate, GlobalWebApiEndpointSourceGenerator.SemanticTransform)
                                        .Where(node => node is not null);

        var globalWebApiEndpointDiagnostics = globalWebApiEndpointData
                                        .Select(static (item, _) => item.Diagnostics)
                                        .Where(static item => item.Count > 0);

        context.RegisterSourceOutput(globalWebApiEndpointDiagnostics, ReportDiagnostic);

        context.RegisterSourceOutput(globalWebApiEndpointData.Collect().Combine(assemblyName),
                                     static (productionContext, source) => GlobalWebApiEndpointSourceGenerator.ExecuteGeneration(productionContext, source.Left, source.Right));
    }

    private static void ReportDiagnostic(SourceProductionContext context, EquatableArray<Diagnostic> diagnostics)
    {
        foreach (var diagnostic in diagnostics)
            context.ReportDiagnostic(diagnostic);
    }
}
