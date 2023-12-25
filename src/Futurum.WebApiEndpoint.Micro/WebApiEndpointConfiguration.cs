using Asp.Versioning;

namespace Futurum.WebApiEndpoint.Micro;

/// <summary>
/// Configuration for WebApiEndpoints
/// </summary>
public record WebApiEndpointConfiguration
{
    /// <summary>
    /// Default WebApiEndpointVersion.
    /// <remarks>This will be used, unless a specified version is specified.</remarks>
    /// </summary>
    public required WebApiEndpointVersion DefaultApiVersion { get; init; }

    public required WebApiEndpointOpenApiConfiguration OpenApi { get; init; }

    public WebApiEndpointVersionConfiguration Version { get; } = new();

    public class WebApiEndpointOpenApiConfiguration
    {
        /// <summary>
        /// Default OpenApiInfo for all versions.
        /// <remarks>Unless overridden by <see cref="VersionedOverrideInfo"/>.</remarks>
        /// </summary>
        public required WebApiEndpointOpenApiInfo DefaultInfo { get; init; }

        /// <summary>
        /// OpenApiInfo overrides for a specific version.
        /// <remarks>If a version is not specified, <see cref="DefaultInfo"/> is used.</remarks>
        /// <remarks>If a property on a version is not specified, that property from <see cref="DefaultInfo"/> is used.</remarks>
        /// </summary>
        public IDictionary<ApiVersion, WebApiEndpointOpenApiInfo> VersionedOverrideInfo { get; } = new Dictionary<ApiVersion, WebApiEndpointOpenApiInfo>();
    }

    public class WebApiEndpointVersionConfiguration
    {
        /// <summary>
        /// Prefix for the versioning
        /// e.g. "v" in "v1.0"
        /// <remarks>Defaults to "v"</remarks>
        /// </summary>
        public string Prefix { get; set; } = "v";

        /// <summary>
        /// See <see cref="Asp.Versioning.ApiVersionFormatProvider"/>.
        /// <remarks>Defaults to "VVVV"</remarks>
        /// </summary>
        public string Format { get; set; } = "VVVV";
    }
}
