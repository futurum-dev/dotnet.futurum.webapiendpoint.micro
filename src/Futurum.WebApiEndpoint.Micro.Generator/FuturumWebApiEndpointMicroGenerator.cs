using System.Text;
using System.Text.RegularExpressions;

using Futurum.WebApiEndpoint.Micro.Generator.Core;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Futurum.WebApiEndpoint.Micro.Generator;

[Generator]
public class FuturumWebApiEndpointMicroGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var webApiEndpointData = context.SyntaxProvider
                                        .CreateSyntaxProvider(WebApiEndpointSourceGenerator.SemanticPredicate, WebApiEndpointSourceGenerator.SemanticTransform)
                                        .Where(node => node is not null);

        var fluentValidatorData = context.SyntaxProvider
                                         .CreateSyntaxProvider(FluentValidatorSourceGenerator.SemanticPredicate, FluentValidatorSourceGenerator.SemanticTransform)
                                         .Where(node => node is not null);

        // Emit the diagnostics, if needed
        var webApiEndpointDiagnostics = webApiEndpointData
                                        .Select(static (item, _) => item.Diagnostics)
                                        .Where(static item => item.Count > 0);

        context.RegisterSourceOutput(webApiEndpointDiagnostics, ReportDiagnostic);

        // Emit the diagnostics, if needed
        var fluentValidatorDiagnostics = fluentValidatorData
                                         .Select(static (item, _) => item.Diagnostics)
                                         .Where(static item => item.Count > 0);

        context.RegisterSourceOutput(fluentValidatorDiagnostics, ReportDiagnostic);

        // include config options
        var assemblyName = context.CompilationProvider
                                  .Select(static (c, _) => c.AssemblyName);

        context.RegisterSourceOutput(assemblyName,
                                     static (productionContext, assemblyName) => ExecuteGeneration(productionContext, assemblyName));

        context.RegisterSourceOutput(webApiEndpointData.Collect().Combine(assemblyName),
                                     static (productionContext, source) => WebApiEndpointSourceGenerator.ExecuteGeneration(productionContext, source.Left, source.Right));

        context.RegisterSourceOutput(fluentValidatorData.Collect().Combine(assemblyName),
                                     static (productionContext, source) => FluentValidatorSourceGenerator.ExecuteGeneration(productionContext, source.Left, source.Right));
    }

    private static void ReportDiagnostic(SourceProductionContext context, EquatableArray<Diagnostic> diagnostics)
    {
        foreach (var diagnostic in diagnostics)
            context.ReportDiagnostic(diagnostic);
    }

    private static void ExecuteGeneration(SourceProductionContext context, string assemblyName)
    {
        var methodName = Regex.Replace(assemblyName, "\\W", "");

        var codeBuilder = SourceGeneratorWriter.Write(assemblyName, methodName);

        context.AddSource("Generator.g.cs", SourceText.From(codeBuilder.ToString(), Encoding.UTF8));
    }
}