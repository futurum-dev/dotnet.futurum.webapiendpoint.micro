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
}
