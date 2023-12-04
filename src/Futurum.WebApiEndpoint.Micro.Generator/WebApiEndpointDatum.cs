using Futurum.WebApiEndpoint.Micro.Generator.Core;

namespace Futurum.WebApiEndpoint.Micro.Generator;

public class WebApiEndpointDatum : IEquatable<WebApiEndpointDatum>
{
    public WebApiEndpointDatum(string namespaceName, string className, string implementationType, string prefix, string tag, IEnumerable<WebApiEndpointVersionDatum> versions)
    {
        NamespaceName = namespaceName;
        ClassName = className;
        ImplementationType = implementationType;
        Prefix = prefix;
        Tag = tag;
        Versions = new EquatableArray<WebApiEndpointVersionDatum>(versions);
    }

    public string NamespaceName { get; }
    public string ClassName { get; }
    public string ImplementationType { get; }
    public string Prefix { get; }
    public string Tag { get; }
    public EquatableArray<WebApiEndpointVersionDatum> Versions { get; }

    public bool Equals(WebApiEndpointDatum? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return NamespaceName == other.NamespaceName &&
               ClassName == other.ClassName &&
               ImplementationType == other.ImplementationType &&
               Prefix == other.Prefix &&
               Tag == other.Tag &&
               Versions.Equals(other.Versions);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((WebApiEndpointDatum)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = NamespaceName.GetHashCode();
            hashCode = (hashCode * 397) ^ ClassName.GetHashCode();
            hashCode = (hashCode * 397) ^ ImplementationType.GetHashCode();
            hashCode = (hashCode * 397) ^ Prefix.GetHashCode();
            hashCode = (hashCode * 397) ^ Tag.GetHashCode();
            hashCode = (hashCode * 397) ^ Versions.GetHashCode();
            return hashCode;
        }
    }
}
