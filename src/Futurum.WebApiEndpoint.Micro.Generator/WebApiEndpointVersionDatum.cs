namespace Futurum.WebApiEndpoint.Micro.Generator;

public class WebApiEndpointVersionDatum : IEquatable<WebApiEndpointVersionDatum>
{
    public WebApiEndpointVersionDatum(int majorVersion, int minorVersion)
    {
        MajorVersion = majorVersion;
        MinorVersion = minorVersion;
    }

    public int MajorVersion { get; }
    public int MinorVersion { get; }

    public bool Equals(WebApiEndpointVersionDatum? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return MajorVersion == other.MajorVersion &&
               MinorVersion == other.MinorVersion;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((WebApiEndpointVersionDatum)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (MajorVersion * 397) ^ MinorVersion;
        }
    }
}
