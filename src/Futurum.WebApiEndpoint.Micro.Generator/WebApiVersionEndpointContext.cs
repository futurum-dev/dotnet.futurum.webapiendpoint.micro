using Futurum.WebApiEndpoint.Micro.Generator.Core;

using Microsoft.CodeAnalysis;

namespace Futurum.WebApiEndpoint.Micro.Generator;

public class WebApiVersionEndpointContext : IEquatable<WebApiVersionEndpointContext>
{
    public WebApiVersionEndpointContext(IEnumerable<Diagnostic>? diagnostics,
                                        WebApiVersionEndpointDatum webApiVersionEndpointData)
    {
        Diagnostics = new EquatableArray<Diagnostic>(diagnostics);
        WebApiVersionEndpointData = webApiVersionEndpointData;
    }

    public EquatableArray<Diagnostic> Diagnostics { get; }
    public WebApiVersionEndpointDatum WebApiVersionEndpointData { get; }

    public bool Equals(WebApiVersionEndpointContext? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Diagnostics.Equals(other.Diagnostics) && WebApiVersionEndpointData.Equals(other.WebApiVersionEndpointData);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((WebApiVersionEndpointContext)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (Diagnostics.GetHashCode() * 397) ^ WebApiVersionEndpointData.GetHashCode();
        }
    }
}