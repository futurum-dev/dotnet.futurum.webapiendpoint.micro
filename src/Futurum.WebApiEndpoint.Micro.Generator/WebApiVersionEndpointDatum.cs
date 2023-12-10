using Futurum.WebApiEndpoint.Micro.Generator.Core;

namespace Futurum.WebApiEndpoint.Micro.Generator;

public class WebApiVersionEndpointDatum : IEquatable<WebApiVersionEndpointDatum>
{
    public WebApiVersionEndpointDatum(string namespaceName, string className, string implementationType, IEnumerable<WebApiEndpointVersionDatum> versions)
    {
        NamespaceName = namespaceName;
        ClassName = className;
        ImplementationType = implementationType;
        Versions = new EquatableArray<WebApiEndpointVersionDatum>(versions);
    }

    public string NamespaceName { get; }
    public string ClassName { get; }
    public string ImplementationType { get; }
    public EquatableArray<WebApiEndpointVersionDatum> Versions { get; }

    public bool Equals(WebApiVersionEndpointDatum? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return NamespaceName == other.NamespaceName &&
               ClassName == other.ClassName &&
               ImplementationType == other.ImplementationType &&
               Versions.Equals(other.Versions);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((WebApiVersionEndpointDatum)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = NamespaceName.GetHashCode();
            hashCode = (hashCode * 397) ^ ClassName.GetHashCode();
            hashCode = (hashCode * 397) ^ ImplementationType.GetHashCode();
            hashCode = (hashCode * 397) ^ Versions.GetHashCode();
            return hashCode;
        }
    }
}
