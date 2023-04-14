using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Futurum.WebApiEndpoint.Micro.Generator;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class WebApiEndpointMethodReturningBadRequestWithoutProblemDetailsAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
        = ImmutableArray.Create(DiagnosticDescriptors.WebApiEndpointMethodReturningBadRequestWithoutProblemDetails);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSymbolAction(Execute, SymbolKind.Method);
    }

    private static void Execute(SymbolAnalysisContext context)
    {
        if (context.Symbol is not IMethodSymbol methodSymbol)
            return;

        var webApiEndpointInterfaceType = Diagnostics.GetWebApiEndpointInterfaceType(context.Compilation);
        if (methodSymbol.ContainingType.AllInterfaces.Contains(webApiEndpointInterfaceType))
        {
            var diagnostics = Diagnostics.WebApiEndpointMethodReturningBadRequestWithoutProblemDetails.Check(methodSymbol);

            foreach (var diagnostic in diagnostics)
            {
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}