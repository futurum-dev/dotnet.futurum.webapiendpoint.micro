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

        var webApiEndpointInterfaceType = Diagnostics.GetWebApiEndpointInterfaceType(semanticModel.Compilation);

        if (classSymbol is not null && webApiEndpointInterfaceType is not null)
        {
            var implementsInterface = classSymbol.AllInterfaces.Contains(webApiEndpointInterfaceType);

            if (implementsInterface)
            {
                var diagnostics = new List<Diagnostic>();
                diagnostics.AddRange(Diagnostics.WebApiEndpointNonEmptyConstructor.Check(classSymbol));

                var webApiEndpointData = new WebApiEndpointDatum(classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));

                return new WebApiEndpointContext(diagnostics, new[] { webApiEndpointData });
            }
        }

        return null;
    }

    public static void ExecuteGeneration(SourceProductionContext context, ImmutableArray<WebApiEndpointContext> webApiEndpointContexts, string assemblyName)
    {
        var methodName = Regex.Replace(assemblyName, "\\W", "");

        var webApiEndpointData = webApiEndpointContexts
                                 .SelectMany(webApiEndpointContext => webApiEndpointContext.WebApiEndpointData)
                                 .Where(registrationData => registrationData is not null)
                                 .ToArray();

        var codeBuilder = WebApiEndpointWriter.Write(methodName, webApiEndpointData);

        context.AddSource("WebApiEndpoints.g.cs", SourceText.From(codeBuilder.ToString(), Encoding.UTF8));
    }
}