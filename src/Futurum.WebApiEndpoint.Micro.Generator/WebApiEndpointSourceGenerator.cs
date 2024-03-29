using System.Collections.Immutable;
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Futurum.WebApiEndpoint.Micro.Generator;

public static class WebApiEndpointSourceGenerator
{
    public static bool SemanticPredicate(SyntaxNode node, CancellationToken ct) =>
        node.IsKind(SyntaxKind.ClassDeclaration);

    public static WebApiEndpointContext? SemanticTransform(GeneratorSyntaxContext context, CancellationToken ct)
    {
        var classDeclaration = (ClassDeclarationSyntax)context.Node;
        var semanticModel = context.SemanticModel;
        var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration, ct);

        if (classSymbol != null)
        {
            var webApiEndpointAttributeData = classSymbol.GetAttributes()
                                                         .FirstOrDefault(attributeData => attributeData.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                                                                                       .StartsWith("global::Futurum.WebApiEndpoint.Micro.WebApiEndpoint")
                                                                                          ?? false);

            if (webApiEndpointAttributeData != null)
            {
                var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
                var className = classSymbol.Name;

                // Get the PrefixRoute and Group property values
                var prefixRoute = GetAttributeArgumentValue(webApiEndpointAttributeData, "PrefixRoute") ?? "/";
                var group = GetAttributeArgumentValue(webApiEndpointAttributeData, "Group") ?? string.Empty;

                var diagnostics = new List<Diagnostic>();
                diagnostics.AddRange(Diagnostics.WebApiEndpointNonEmptyConstructor.Check(classSymbol));

                var apiVersions = GetApiVersions(classSymbol);

                var webApiEndpointData = new WebApiEndpointDatum(namespaceName,
                                                                 className,
                                                                 classSymbol.Name,
                                                                 prefixRoute,
                                                                 group,
                                                                 apiVersions.Select(apiVersion => new WebApiEndpointVersionDatum(apiVersion)));

                return new WebApiEndpointContext(diagnostics, webApiEndpointData);
            }
        }

        return null;
    }

    private static IEnumerable<WebApiEndpointApiVersion> GetApiVersions(INamedTypeSymbol classSymbol)
    {
        var webApiEndpointVersionAttributeData = classSymbol.GetAttributes()
                                                            .Where(attributeData => attributeData.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                                                                                 .StartsWith("global::Futurum.WebApiEndpoint.Micro.WebApiEndpointVersion")
                                                                                    ?? false);

        foreach (var webApiEndpointVersionAttributeDatum in webApiEndpointVersionAttributeData)
        {
            var apiVersion = webApiEndpointVersionAttributeDatum.ToWebApiEndpointApiVersion();

            if (apiVersion != null)
            {
                yield return apiVersion;
            }
        }
    }

    private static string? GetAttributeArgumentValue(AttributeData attributeData, string argumentName)
    {
        string? result = default;

        // Check if the argument is a positional argument
        if (argumentName == "PrefixRoute" && attributeData.ConstructorArguments.Length > 0)
        {
            result = attributeData.ConstructorArguments[0].Value as string;
        }
        else if (argumentName == "Group" && attributeData.ConstructorArguments.Length > 1)
        {
            result = attributeData.ConstructorArguments[1].Value as string;
        }

        return result;
    }

    public static void ExecuteGeneration(SourceProductionContext context, ImmutableArray<WebApiEndpointContext> webApiEndpointContexts, string assemblyName)
    {
        var methodName = Regex.Replace(assemblyName, "\\W", "");

        RegistrationGeneration(context, webApiEndpointContexts, methodName);

        PartialClassGeneration(context, webApiEndpointContexts, methodName);
    }

    private static void RegistrationGeneration(SourceProductionContext context, ImmutableArray<WebApiEndpointContext> webApiEndpointContexts, string methodName)
    {
        var webApiEndpointData = webApiEndpointContexts
                                 .Select(webApiEndpointContext => webApiEndpointContext.WebApiEndpointData)
                                 .ToArray();

        var codeBuilder = WebApiEndpointRegistrationWriter.Write(methodName, webApiEndpointData);

        context.AddSource("WebApiEndpoints.g.cs", SourceText.From(codeBuilder, Encoding.UTF8));
    }

    private static void PartialClassGeneration(SourceProductionContext context, ImmutableArray<WebApiEndpointContext> webApiEndpointContexts, string methodName)
    {
        var webApiEndpointData = webApiEndpointContexts
                                 .Select(webApiEndpointContext => webApiEndpointContext.WebApiEndpointData)
                                 .ToArray();

        foreach (var webApiEndpointDatum in webApiEndpointData)
        {
            var codeBuilder = WebApiEndpointPartialClassWriter.Write(webApiEndpointDatum);

            var fileName = $"{webApiEndpointDatum.NamespaceName}.{webApiEndpointDatum.ImplementationType}.WebApiEndpoint.g.cs";
            context.AddSource(fileName, SourceText.From(codeBuilder, Encoding.UTF8));
        }
    }
}
