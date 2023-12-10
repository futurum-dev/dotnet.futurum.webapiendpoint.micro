namespace Futurum.WebApiEndpoint.Micro.Generator;

public class WebApiEndpointVersionDatum : IEquatable<WebApiEndpointVersionDatum>
{
    public WebApiEndpointVersionDatum(WebApiEndpointApiVersion apiVersion)
    {
        ApiVersion = apiVersion;
    }

    public WebApiEndpointApiVersion ApiVersion { get; }

    public bool Equals(WebApiEndpointVersionDatum? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return ApiVersion.Equals(other.ApiVersion);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((WebApiEndpointVersionDatum)obj);
    }

    public override int GetHashCode() =>
        ApiVersion.GetHashCode();
}
