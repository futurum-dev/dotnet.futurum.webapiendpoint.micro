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

        var implementingClasses = new Dictionary<(int Major, int Minor), List<INamedTypeSymbol>>();

        context.RegisterSymbolAction(symbolContext =>
            {
                var namedTypeSymbol = (INamedTypeSymbol)symbolContext.Symbol;
                if (namedTypeSymbol.TypeKind == TypeKind.Class && namedTypeSymbol.Interfaces.Contains(webApiVersionEndpointInterfaceSymbol, SymbolEqualityComparer.Default))
                {
                    var attribute = namedTypeSymbol.GetAttributes().FirstOrDefault(attr => SymbolEqualityComparer.Default.Equals(attr.AttributeClass, webApiVersionEndpointVersionAttributeSymbol));
                    if (attribute != null)
                    {
                        // Get the MajorVersion and MinorVersion property values
                        var majorVersion = GetAttributeVersionArgumentValue(attribute, "MajorVersion") ?? 0;
                        var minorVersion = GetAttributeVersionArgumentValue(attribute, "MinorVersion") ?? 0;
                        var version = (majorVersion, minorVersion);

                        if (!implementingClasses.TryGetValue(version, out var list))
                        {
                            list = new List<INamedTypeSymbol>();
                            implementingClasses[version] = list;
                        }

                        list.Add(namedTypeSymbol);
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
                            var diagnostics = Diagnostics.WebApiVersionEndpointMoreThanOneInstance.Check(namedTypeSymbol, pair.Key.Major, pair.Key.Minor);
                            foreach (var diagnostic in diagnostics)
                            {
                                compilationContext.ReportDiagnostic(diagnostic);
                            }
                        }
                    }
                }
            });
    }

    private static int? GetAttributeVersionArgumentValue(AttributeData attributeData, string argumentName)
    {
        int? result = default;

        // Check if the argument is a positional argument
        if (argumentName == "MajorVersion" && attributeData.ConstructorArguments.Length > 0)
        {
            result = attributeData.ConstructorArguments[0].Value as int?;
        }
        else if (argumentName == "MinorVersion" && attributeData.ConstructorArguments.Length > 1)
        {
            result = attributeData.ConstructorArguments[1].Value as int?;
        }

        return result;
    }
}
