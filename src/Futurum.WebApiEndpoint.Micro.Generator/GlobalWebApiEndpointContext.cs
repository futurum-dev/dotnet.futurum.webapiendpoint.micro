using Futurum.WebApiEndpoint.Micro.Generator.Core;

using Microsoft.CodeAnalysis;

namespace Futurum.WebApiEndpoint.Micro.Generator;

public class GlobalWebApiEndpointContext : IEquatable<GlobalWebApiEndpointContext>
{
    public GlobalWebApiEndpointContext(IEnumerable<Diagnostic>? diagnostics,
                                       GlobalWebApiEndpointDatum globalWebApiEndpointData)
    {
        Diagnostics = new EquatableArray<Diagnostic>(diagnostics);
        GlobalWebApiEndpointData = globalWebApiEndpointData;
    }

    public EquatableArray<Diagnostic> Diagnostics { get; }
    public GlobalWebApiEndpointDatum GlobalWebApiEndpointData { get; }

    public bool Equals(GlobalWebApiEndpointContext? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Diagnostics.Equals(other.Diagnostics) &&
               GlobalWebApiEndpointData.Equals(other.GlobalWebApiEndpointData);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((GlobalWebApiEndpointContext)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (Diagnostics.GetHashCode() * 397) ^ GlobalWebApiEndpointData.GetHashCode();
        }
    }
}
