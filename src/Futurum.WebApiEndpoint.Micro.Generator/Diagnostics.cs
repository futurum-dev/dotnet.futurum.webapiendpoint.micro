using Microsoft.CodeAnalysis;

namespace Futurum.WebApiEndpoint.Micro.Generator;

public static class Diagnostics
{
    public static INamedTypeSymbol? GetWebApiEndpointInterfaceType(Compilation semanticModelCompilation) =>
        semanticModelCompilation.GetTypeByMetadataName("Futurum.WebApiEndpoint.Micro.IWebApiEndpoint");

    public static INamedTypeSymbol? GetWebApiVersionEndpointInterfaceType(Compilation semanticModelCompilation) =>
        semanticModelCompilation.GetTypeByMetadataName("Futurum.WebApiEndpoint.Micro.IWebApiVersionEndpoint");

    public static INamedTypeSymbol? GetWebApiVersionEndpointVersionAttributeType(Compilation semanticModelCompilation) =>
        semanticModelCompilation.GetTypeByMetadataName("Futurum.WebApiEndpoint.Micro.WebApiVersionEndpointVersionAttribute");

    public static INamedTypeSymbol? GetGlobalWebApiEndpointInterfaceType(Compilation semanticModelCompilation) =>
        semanticModelCompilation.GetTypeByMetadataName("Futurum.WebApiEndpoint.Micro.IGlobalWebApiEndpoint");

    public static class WebApiEndpointNonEmptyConstructor
    {
        public static IEnumerable<Diagnostic> Check(INamedTypeSymbol classSymbol)
        {
            foreach (var classSymbolConstructor in classSymbol.Constructors)
            {
                var emptyConstructor = !classSymbolConstructor.Parameters.Any();

                if (!emptyConstructor)
                {
                    yield return Diagnostic.Create(DiagnosticDescriptors.WebApiEndpointNonEmptyConstructor,
                                                   classSymbolConstructor.Locations.First(),
                                                   classSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat));
                }
            }
        }
    }

    public static class WebApiEndpointMethodReturningBadRequestWithoutProblemDetails
    {
        public static IEnumerable<Diagnostic> Check(IMethodSymbol methodSymbol)
        {
            var returnType = methodSymbol.ReturnType;

            if (!returnType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).StartsWith("global::Microsoft.AspNetCore.Http.HttpResults.Results"))
                yield break;

            if (returnType is not INamedTypeSymbol namedTypeSymbol)
                yield break;

            foreach (var typeSymbol in namedTypeSymbol.TypeArguments)
            {
                if (typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).StartsWith("global::Microsoft.AspNetCore.Http.HttpResults.BadRequest"))
                {
                    if (typeSymbol is INamedTypeSymbol typeSymbolOriginalDefinition)
                    {
                        var returnTypeArgument = typeSymbolOriginalDefinition.TypeArguments.First();

                        if (returnTypeArgument.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) != "global::Microsoft.AspNetCore.Mvc.ProblemDetails")
                        {
                            yield return Diagnostic.Create(DiagnosticDescriptors.WebApiEndpointMethodReturningBadRequestWithoutProblemDetails,
                                                           methodSymbol.Locations.First());
                        }
                    }
                }
            }
        }
    }

    public static class GlobalWebApiEndpointMoreThanOneInstance
    {
        public static IEnumerable<Diagnostic> Check(INamedTypeSymbol classSymbol)
        {
            yield return Diagnostic.Create(DiagnosticDescriptors.GlobalWebApiEndpointMultipleInstances,
                                           classSymbol.Locations.First(),
                                           classSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat));
        }
    }

    public static class WebApiVersionEndpointMoreThanOneInstance
    {
        public static IEnumerable<Diagnostic> Check(INamedTypeSymbol classSymbol, int majorVersion, int minorVersion)
        {
            yield return Diagnostic.Create(DiagnosticDescriptors.WebApiVersionEndpointMultipleInstances,
                                           classSymbol.Locations.First(),
                                           classSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat),
                                           majorVersion, minorVersion);
        }
    }
}
