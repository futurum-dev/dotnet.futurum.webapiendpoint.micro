using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Futurum.WebApiEndpoint.Micro.Generator;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class WebApiVersionEndpointSingleInstanceAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
        = ImmutableArray.Create(DiagnosticDescriptors.WebApiVersionEndpointMultipleInstances);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterCompilationStartAction(Execute);
    }

    private static void Execute(CompilationStartAnalysisContext context)
    {
        var webApiVersionEndpointInterfaceSymbol = Diagnostics.GetWebApiVersionEndpointInterfaceType(context.Compilation);
        var webApiVersionEndpointVersionAttributeSymbol = Diagnostics.GetWebApiVersionEndpointVersionAttributeType(context.Compilation);

        var implementingClasses = new Dictionary<WebApiEndpointApiVersion, List<INamedTypeSymbol>>();

        context.RegisterSymbolAction(symbolContext =>
        {
            var namedTypeSymbol = (INamedTypeSymbol)symbolContext.Symbol;
            if (namedTypeSymbol.TypeKind == TypeKind.Class &&
                namedTypeSymbol.Interfaces.Contains(webApiVersionEndpointInterfaceSymbol, SymbolEqualityComparer.Default))
            {
                var attribute = namedTypeSymbol.GetAttributes()
                                               .FirstOrDefault(attr => SymbolEqualityComparer.Default.Equals(attr.AttributeClass, webApiVersionEndpointVersionAttributeSymbol));
                if (attribute != null)
                {
                    var apiVersion = attribute.ToWebApiEndpointApiVersion();

                    if (apiVersion != null)
                    {
                        if (!implementingClasses.TryGetValue(apiVersion, out var list))
                        {
                            list = new List<INamedTypeSymbol>();
                            implementingClasses[apiVersion] = list;
                        }

                        list.Add(namedTypeSymbol);
                    }
                }
            }
        }, SymbolKind.NamedType);

        context.RegisterCompilationEndAction(compilationContext =>
        {
            foreach (var pair in implementingClasses)
            {
                if (pair.Value.Count > 1)
                {
                    foreach (var namedTypeSymbol in pair.Value)
                    {
                        var diagnostics = Diagnostics.WebApiVersionEndpointMoreThanOneInstance.Check(namedTypeSymbol, pair.Key.ToString());
                        foreach (var diagnostic in diagnostics)
                        {
                            compilationContext.ReportDiagnostic(diagnostic);
                        }
                    }
                }
            }
        });
    }
}
