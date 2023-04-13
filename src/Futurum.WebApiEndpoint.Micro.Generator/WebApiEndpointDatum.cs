using Futurum.WebApiEndpoint.Micro.Generator.Core;

namespace Futurum.WebApiEndpoint.Micro.Generator;

public class WebApiEndpointDatum : IEquatable<WebApiEndpointDatum>
{
    public WebApiEndpointDatum(string implementationType)
    {
        ImplementationType = implementationType;
    }

    public string ImplementationType { get; }

    public bool Equals(WebApiEndpointDatum? other)
    {
        if (ReferenceEquals(null, other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return ImplementationType.Equals(other.ImplementationType);
    }

    public override bool Equals(object? obj) =>
        obj is WebApiEndpointDatum webApiEndpointData && Equals(webApiEndpointData);

    public override int GetHashCode() =>
        HashCode.Combine(ImplementationType);
}