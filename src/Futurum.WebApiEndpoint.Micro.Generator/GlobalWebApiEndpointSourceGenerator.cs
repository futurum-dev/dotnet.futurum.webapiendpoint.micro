using System.Collections.Immutable;
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Futurum.WebApiEndpoint.Micro.Generator;

public static class GlobalWebApiEndpointSourceGenerator
{
    public static bool SemanticPredicate(SyntaxNode node, CancellationToken ct) =>
        node.IsKind(SyntaxKind.ClassDeclaration);

    public static GlobalWebApiEndpointContext? SemanticTransform(GeneratorSyntaxContext context, CancellationToken ct)
    {
        var classDeclaration = (ClassDeclarationSyntax)context.Node;
        var semanticModel = context.SemanticModel;
        var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration, ct);

        if (classSymbol != null)
        {
            var webApiVersionEndpointAttributeData = classSymbol.GetAttributes()
                                                                .FirstOrDefault(attributeData => attributeData.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                                                                                              .StartsWith("global::Futurum.WebApiEndpoint.Micro.GlobalWebApiEndpoint")
                                                                                                 ?? false);

            if (webApiVersionEndpointAttributeData != null)
            {
                var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
                var className = classSymbol.Name;

                var diagnostics = new List<Diagnostic>();
                diagnostics.AddRange(Diagnostics.WebApiEndpointNonEmptyConstructor.Check(classSymbol));

                var webApiEndpointData = new GlobalWebApiEndpointDatum(namespaceName,
                                                                       className,
                                                                       classSymbol.Name);

                return new GlobalWebApiEndpointContext(diagnostics, webApiEndpointData);
            }
        }

        return null;
    }

    public static void ExecuteGeneration(SourceProductionContext context, ImmutableArray<GlobalWebApiEndpointContext> globalWebApiEndpointContexts, string assemblyName)
    {
        var methodName = Regex.Replace(assemblyName, "\\W", "");

        RegistrationGeneration(context, globalWebApiEndpointContexts, methodName);
    }

    private static void RegistrationGeneration(SourceProductionContext context, ImmutableArray<GlobalWebApiEndpointContext> globalWebApiEndpointContexts, string methodName)
    {
        var globalWebApiEndpointDatum = globalWebApiEndpointContexts
                                        .Select(webApiEndpointContext => webApiEndpointContext.GlobalWebApiEndpointData)
                                        .FirstOrDefault();

        var codeBuilder = GlobalWebApiEndpointRegistrationWriter.Write(methodName, globalWebApiEndpointDatum);

        context.AddSource("GlobalWebApiEndpoint.g.cs", SourceText.From(codeBuilder, Encoding.UTF8));
    }
}
