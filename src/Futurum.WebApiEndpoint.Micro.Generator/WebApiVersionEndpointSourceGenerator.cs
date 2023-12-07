using System.Collections.Immutable;
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Futurum.WebApiEndpoint.Micro.Generator;

public static class WebApiVersionEndpointSourceGenerator
{
    public static bool SemanticPredicate(SyntaxNode node, CancellationToken ct) =>
        node.IsKind(SyntaxKind.ClassDeclaration);

    public static WebApiVersionEndpointContext? SemanticTransform(GeneratorSyntaxContext context, CancellationToken ct)
    {
        var classDeclaration = (ClassDeclarationSyntax)context.Node;
        var semanticModel = context.SemanticModel;
        var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration, ct);

        if (classSymbol != null)
        {
            var webApiVersionEndpointAttributeData = classSymbol.GetAttributes()
                                                         .FirstOrDefault(attributeData => attributeData.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                                                                                       .StartsWith("global::Futurum.WebApiEndpoint.Micro.WebApiVersionEndpoint")
                                                                                          ?? false);

            if (webApiVersionEndpointAttributeData != null)
            {
                var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
                var className = classSymbol.Name;

                var diagnostics = new List<Diagnostic>();
                diagnostics.AddRange(Diagnostics.WebApiEndpointNonEmptyConstructor.Check(classSymbol));

                var versions = GetVersion(classSymbol);

                var webApiEndpointData = new WebApiVersionEndpointDatum(namespaceName,
                                                                        className,
                                                                        classSymbol.Name,
                                                                        versions.Select(version => new WebApiEndpointVersionDatum(version.majorVersion, version.minorVersion)));

                return new WebApiVersionEndpointContext(diagnostics, webApiEndpointData);
            }
        }

        return null;
    }

    private static IEnumerable<(int majorVersion, int minorVersion)> GetVersion(INamedTypeSymbol classSymbol)
    {
        var webApiEndpointVersionAttributeData = classSymbol.GetAttributes()
                                                            .Where(attributeData => attributeData.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                                                                                 .StartsWith("global::Futurum.WebApiEndpoint.Micro.WebApiVersionEndpointVersion")
                                                                                    ?? false);

        foreach (var webApiEndpointVersionAttributeDatum in webApiEndpointVersionAttributeData)
        {
            int? majorVersion = null;
            int? minorVersion = null;

            if (webApiEndpointVersionAttributeData != null)
            {
                // Get the MajorVersion and MinorVersion property values
                majorVersion = GetAttributeVersionArgumentValue(webApiEndpointVersionAttributeDatum, "MajorVersion");
                minorVersion = GetAttributeVersionArgumentValue(webApiEndpointVersionAttributeDatum, "MinorVersion");
            }

            if (majorVersion.HasValue && minorVersion.HasValue)
            {
                yield return (majorVersion.Value, minorVersion.Value);
            }
        }
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

    public static void ExecuteGeneration(SourceProductionContext context, ImmutableArray<WebApiVersionEndpointContext> webApiVersionEndpointContexts, string assemblyName)
    {
        var methodName = Regex.Replace(assemblyName, "\\W", "");

        RegistrationGeneration(context, webApiVersionEndpointContexts, methodName);
    }

    private static void RegistrationGeneration(SourceProductionContext context, ImmutableArray<WebApiVersionEndpointContext> webApiVersionsEndpointContexts, string methodName)
    {
        var webApiVersionEndpointData = webApiVersionsEndpointContexts
                                 .Select(webApiEndpointContext => webApiEndpointContext.WebApiVersionEndpointData)
                                 .ToArray();

        var codeBuilder = WebApiVersionEndpointRegistrationWriter.Write(methodName, webApiVersionEndpointData);

        context.AddSource("WebApiVersionEndpoints.g.cs", SourceText.From(codeBuilder, Encoding.UTF8));
    }
}
