namespace Futurum.WebApiEndpoint.Micro.Generator;

public class GlobalWebApiEndpointDatum : IEquatable<GlobalWebApiEndpointDatum>
{
    public GlobalWebApiEndpointDatum(string namespaceName, string className, string implementationType)
    {
        NamespaceName = namespaceName;
        ClassName = className;
        ImplementationType = implementationType;
    }

    public string NamespaceName { get; }
    public string ClassName { get; }
    public string ImplementationType { get; }

    public bool Equals(GlobalWebApiEndpointDatum? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return NamespaceName == other.NamespaceName && 
               ClassName == other.ClassName && 
               ImplementationType == other.ImplementationType;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((GlobalWebApiEndpointDatum)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = NamespaceName.GetHashCode();
            hashCode = (hashCode * 397) ^ ClassName.GetHashCode();
            hashCode = (hashCode * 397) ^ ImplementationType.GetHashCode();
            return hashCode;
        }
    }
}