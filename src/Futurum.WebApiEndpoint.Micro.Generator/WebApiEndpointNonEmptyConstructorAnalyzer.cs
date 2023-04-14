using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Futurum.WebApiEndpoint.Micro.Generator;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class WebApiEndpointNonEmptyConstructorAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
        = ImmutableArray.Create(DiagnosticDescriptors.WebApiEndpointNonEmptyConstructor);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSymbolAction(Execute, SymbolKind.NamedType);
    }

    private static void Execute(SymbolAnalysisContext context)
    {
        if (context.Symbol is not INamedTypeSymbol classSymbol)
            return;

        var webApiEndpointInterfaceType = Diagnostics.GetWebApiEndpointInterfaceType(context.Compilation);
        if (webApiEndpointInterfaceType is not null)
        {
            var implementsInterface = classSymbol.AllInterfaces.Contains(webApiEndpointInterfaceType);

            if (implementsInterface)
            {
                var diagnostics = Diagnostics.WebApiEndpointNonEmptyConstructor.Check(classSymbol);

                foreach (var diagnostic in diagnostics)
                {
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}