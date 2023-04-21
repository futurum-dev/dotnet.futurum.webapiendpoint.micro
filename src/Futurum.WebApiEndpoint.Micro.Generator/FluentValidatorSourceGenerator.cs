using System.Collections.Immutable;
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Futurum.WebApiEndpoint.Micro.Generator;

public static class FluentValidatorSourceGenerator
{
    public static bool SemanticPredicate(SyntaxNode node, CancellationToken _) =>
        node.IsKind(SyntaxKind.ClassDeclaration);

    public static FluentValidatorContext? SemanticTransform(GeneratorSyntaxContext context, CancellationToken ct)
    {
        if (context.Node is ClassDeclarationSyntax classDeclarationSyntax)
        {
            var semanticModel = context.SemanticModel;
            var classSymbol = semanticModel.GetDeclaredSymbol(classDeclarationSyntax, ct);

            if (classSymbol != null)
            {
                var interfaceNamedTypeSymbol = classSymbol.AllInterfaces
                                                          .FirstOrDefault(t => t.ContainingNamespace.MetadataName == "FluentValidation" && t.MetadataName == "IValidator`1");
                if (interfaceNamedTypeSymbol != null)
                {
                    var fluentValidatorDatum = new FluentValidatorDatum(interfaceNamedTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                                                                        classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
                    return new FluentValidatorContext(fluentValidatorData: new[] { fluentValidatorDatum });
                }
            }

            return null;
        }

        return null;
    }

    public static void ExecuteGeneration(SourceProductionContext context, ImmutableArray<FluentValidatorContext> fluentValidatorContexts, string assemblyName)
    {
        var methodName = Regex.Replace(assemblyName, "\\W", "");

        var fluentValidatorData = fluentValidatorContexts
                                  .SelectMany(webApiEndpointContext => webApiEndpointContext.FluentValidatorData)
                                  .Where(registrationData => registrationData is not null)
                                  .ToArray();

        var codeBuilder = FluentValidatorWriter.Write(methodName, fluentValidatorData);

        context.AddSource("FluentValidators.g.cs", SourceText.From(codeBuilder.ToString(), Encoding.UTF8));
    }
}