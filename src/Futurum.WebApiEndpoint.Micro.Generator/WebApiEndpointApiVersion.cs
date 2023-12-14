namespace Futurum.WebApiEndpoint.Micro.Generator;

public abstract class WebApiEndpointApiVersion
{
    public class WebApiEndpointNumberApiVersion : WebApiEndpointApiVersion,
                                                  IEquatable<WebApiEndpointNumberApiVersion>
    {
        public WebApiEndpointNumberApiVersion(double version, string? status = default)
        {
            Version = version;
            Status = status;
        }

        public double Version { get; }
        public string? Status { get; }

        public bool Equals(WebApiEndpointNumberApiVersion? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Version.Equals(other.Version) && Status == other.Status;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WebApiEndpointNumberApiVersion)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Version.GetHashCode() * 397) ^ (Status != null ? Status.GetHashCode() : 0);
            }
        }

        public static bool operator ==(WebApiEndpointNumberApiVersion? left, WebApiEndpointNumberApiVersion? right) =>
            Equals(left, right);

        public static bool operator !=(WebApiEndpointNumberApiVersion? left, WebApiEndpointNumberApiVersion? right) =>
            !Equals(left, right);
    }

    public class WebApiEndpointStringApiVersion : WebApiEndpointApiVersion,
                                                  IEquatable<WebApiEndpointStringApiVersion>
    {
        public WebApiEndpointStringApiVersion(string version)
        {
            Version = version;
        }

        public string Version { get; }

        public bool Equals(WebApiEndpointStringApiVersion? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Version == other.Version;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WebApiEndpointStringApiVersion)obj);
        }

        public override int GetHashCode() =>
            Version.GetHashCode();

        public static bool operator ==(WebApiEndpointStringApiVersion? left, WebApiEndpointStringApiVersion? right) =>
            Equals(left, right);

        public static bool operator !=(WebApiEndpointStringApiVersion? left, WebApiEndpointStringApiVersion? right) =>
            !Equals(left, right);
    }
}
