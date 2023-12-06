using Futurum.WebApiEndpoint.Micro.Generator.Core;

using Microsoft.CodeAnalysis;

namespace Futurum.WebApiEndpoint.Micro.Generator;

public class WebApiEndpointContext : IEquatable<WebApiEndpointContext>
{
    public WebApiEndpointContext(IEnumerable<Diagnostic>? diagnostics,
                                 WebApiEndpointDatum webApiEndpointData)
    {
        Diagnostics = new EquatableArray<Diagnostic>(diagnostics);
        WebApiEndpointData = webApiEndpointData;
    }

    public EquatableArray<Diagnostic> Diagnostics { get; }
    public WebApiEndpointDatum WebApiEndpointData { get; }

    public bool Equals(WebApiEndpointContext? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Diagnostics.Equals(other.Diagnostics) &&
               WebApiEndpointData.Equals(other.WebApiEndpointData);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((WebApiEndpointContext)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (Diagnostics.GetHashCode() * 397) ^ WebApiEndpointData.GetHashCode();
        }
    }
}
