using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Futurum.WebApiEndpoint.Micro.Generator;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class GlobalWebApiEndpointSingleInstanceAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
        = ImmutableArray.Create(DiagnosticDescriptors.GlobalWebApiEndpointMultipleInstances);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterCompilationStartAction(Execute);
    }

    private static void Execute(CompilationStartAnalysisContext context)
    {
        var globalWebApiEndpointInterfaceSymbol = Diagnostics.GetGlobalWebApiEndpointInterfaceType(context.Compilation);

        var implementingClasses = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);

        context.RegisterSymbolAction(symbolContext =>
        {
            var namedTypeSymbol = (INamedTypeSymbol)symbolContext.Symbol;
            if (namedTypeSymbol.TypeKind == TypeKind.Class && namedTypeSymbol.Interfaces.Contains(globalWebApiEndpointInterfaceSymbol, SymbolEqualityComparer.Default))
            {
                implementingClasses.Add(namedTypeSymbol);
            }
        }, SymbolKind.NamedType);

        context.RegisterCompilationEndAction(compilationContext =>
        {
            if (implementingClasses.Count > 1)
            {
                foreach (var namedTypeSymbol in implementingClasses)
                {
                    var diagnostics = Diagnostics.GlobalWebApiEndpointMoreThanOneInstance.Check(namedTypeSymbol);
                    foreach (var diagnostic in diagnostics)
                    {
                        compilationContext.ReportDiagnostic(diagnostic);
                    }
                }
            }
        });
    }
}
