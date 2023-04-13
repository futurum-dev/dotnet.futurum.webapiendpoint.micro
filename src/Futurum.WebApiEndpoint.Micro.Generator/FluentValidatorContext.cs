using Futurum.WebApiEndpoint.Micro.Generator.Core;

using Microsoft.CodeAnalysis;

namespace Futurum.WebApiEndpoint.Micro.Generator;

public class FluentValidatorContext : IEquatable<FluentValidatorContext>
{
    public FluentValidatorContext(IEnumerable<Diagnostic>? diagnostics = null,
                                  IEnumerable<FluentValidatorDatum>? fluentValidatorData = null)
    {
        Diagnostics = new EquatableArray<Diagnostic>(diagnostics);
        FluentValidatorData = new EquatableArray<FluentValidatorDatum>(fluentValidatorData);
    }

    public EquatableArray<Diagnostic> Diagnostics { get; }
    public EquatableArray<FluentValidatorDatum> FluentValidatorData { get; }

    public bool Equals(FluentValidatorContext? other)
    {
        if (ReferenceEquals(null, other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return Diagnostics == other.Diagnostics
               && FluentValidatorData == other.FluentValidatorData;
    }

    public override bool Equals(object? obj) =>
        obj is FluentValidatorContext fluentValidatorContext && Equals(fluentValidatorContext);

    public override int GetHashCode() =>
        HashCode.Combine(Diagnostics, FluentValidatorData);
}