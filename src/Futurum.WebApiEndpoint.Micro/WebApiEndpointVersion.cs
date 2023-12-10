using Asp.Versioning;

using Futurum.WebApiEndpoint.Micro.Generator;

namespace Futurum.WebApiEndpoint.Micro;

public record WebApiEndpointVersion(WebApiEndpointApiVersion ApiVersion)
{
    public static implicit operator ApiVersion(WebApiEndpointVersion value) =>
        value.ApiVersion switch
        {
            WebApiEndpointApiVersion.WebApiEndpointNumberApiVersion numberApiVersion => new(numberApiVersion.Version, numberApiVersion.Status),
            WebApiEndpointApiVersion.WebApiEndpointStringApiVersion stringApiVersion => ApiVersionParser.Default.Parse(stringApiVersion.Version),
            _                                                                        => throw new InvalidOperationException($"Unknown {nameof(WebApiEndpointApiVersion)} type: {value.ApiVersion.GetType().FullName}")
        };

    public static WebApiEndpointVersion Create(double version, string? status = default) =>
        new(new WebApiEndpointApiVersion.WebApiEndpointNumberApiVersion(version, status));

    public static WebApiEndpointVersion Create(string version) =>
        new(new WebApiEndpointApiVersion.WebApiEndpointStringApiVersion(version));
}
