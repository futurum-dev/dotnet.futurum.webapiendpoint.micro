using Futurum.WebApiEndpoint.Micro.Generator.Core;

namespace Futurum.WebApiEndpoint.Micro.Generator;

public class FluentValidatorDatum : IEquatable<FluentValidatorDatum>
{
    public FluentValidatorDatum(string interfaceType, string implementationType)
    {
        InterfaceType = interfaceType;
        ImplementationType = implementationType;
    }

    public string InterfaceType { get; }
    public string ImplementationType { get; }

    public bool Equals(FluentValidatorDatum? other)
    {
        if (ReferenceEquals(null, other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return InterfaceType.Equals(other.InterfaceType)
               && ImplementationType.Equals(other.ImplementationType);
    }

    public override bool Equals(object? obj) =>
        obj is FluentValidatorDatum fluentValidatorDatum && Equals(fluentValidatorDatum);

    public override int GetHashCode() =>
        HashCode.Combine(InterfaceType, ImplementationType);
}