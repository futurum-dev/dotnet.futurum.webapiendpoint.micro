using Microsoft.CodeAnalysis;

namespace Futurum.WebApiEndpoint.Micro.Generator;

public static class DiagnosticDescriptors
{
    public static readonly DiagnosticDescriptor WebApiEndpointNonEmptyConstructor = new(
        "FWAEM0001",
        "Non empty constructor found on WebApiEndpoint",
        $"WebApiEndpoint class '{{0}}' does not have an empty constructor.\n" +
        $"We recommend that WebApiEndpoint's have an empty constructor and to take any injectable dependencies as parameters via the minimal API method itself.\n" +
        $"Constructor dependencies will have a lifetime outside of the minimal API lifetime and could have unintended consequences.",
        "Futurum.WebApiEndpoint.Micro.Generator",
        DiagnosticSeverity.Warning,
        true);

    public static readonly DiagnosticDescriptor WebApiEndpointMethodReturningBadRequestWithoutProblemDetails = new(
        "FWAEM0002",
        "BadRequest without 'ProblemDetails' use found on WebApiEndpoint",
        "Minimal API methods returning a 'BadRequest', should ensure that the 'BadRequest' is created with a 'ProblemDetails' instance.",
        "Futurum.WebApiEndpoint.Micro.Generator",
        DiagnosticSeverity.Warning,
        true);

    public static readonly DiagnosticDescriptor GlobalWebApiEndpointMultipleInstances = new(
        "FWAEM0003",
        "Multiple instances found of GlobalWebApiEndpoint",
        $"GlobalWebApiEndpoint class '{{0}}' is one of many instances.\n" +
        $"There can only be one instance of GlobalWebApiEndpoint.",
        "Futurum.WebApiEndpoint.Micro.Generator",
        DiagnosticSeverity.Error,
        true);

    public static readonly DiagnosticDescriptor WebApiVersionEndpointMultipleInstances = new(
        "FWAEM0004",
        "Multiple instances found of WebApiVersionEndpoint for the same version",
        $"WebApiVersionEndpoint class '{{0}}' with version '{{1}}' is one of many instances.\n" +
        $"There can only be one instance of WebApiVersionEndpoint.",
        "Futurum.WebApiEndpoint.Micro.Generator",
        DiagnosticSeverity.Error,
        true);
}
