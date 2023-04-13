using Futurum.WebApiEndpoint.Micro.Generator.Core;

using Microsoft.CodeAnalysis;

namespace Futurum.WebApiEndpoint.Micro.Generator;

public class WebApiEndpointContext : IEquatable<WebApiEndpointContext>
{
    public WebApiEndpointContext(IEnumerable<Diagnostic>? diagnostics = null,
                                 IEnumerable<WebApiEndpointDatum>? webApiEndpointData = null)
    {
        Diagnostics = new EquatableArray<Diagnostic>(diagnostics);
        WebApiEndpointData = new EquatableArray<WebApiEndpointDatum>(webApiEndpointData);
    }

    public EquatableArray<Diagnostic> Diagnostics { get; }
    public EquatableArray<WebApiEndpointDatum> WebApiEndpointData { get; }

    public bool Equals(WebApiEndpointContext? other)
    {
        if (ReferenceEquals(null, other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return Diagnostics == other.Diagnostics
               && WebApiEndpointData == other.WebApiEndpointData;
    }

    public override bool Equals(object? obj) =>
        obj is WebApiEndpointContext webApiEndpointContext && Equals(webApiEndpointContext);

    public override int GetHashCode() =>
        HashCode.Combine(Diagnostics, WebApiEndpointData);
}