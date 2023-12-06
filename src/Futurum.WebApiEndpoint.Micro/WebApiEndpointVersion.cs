using Asp.Versioning;

namespace Futurum.WebApiEndpoint.Micro;

public record WebApiEndpointVersion(int MajorVersion, int MinorVersion)
{
    public static implicit operator ApiVersion(WebApiEndpointVersion value) =>
        new(value.MajorVersion, value.MinorVersion);
}
