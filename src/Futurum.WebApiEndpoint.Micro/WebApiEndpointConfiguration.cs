using Asp.Versioning;

using Microsoft.OpenApi.Models;

namespace Futurum.WebApiEndpoint.Micro;

/// <summary>
/// Configuration for WebApiEndpoints
/// </summary>
/// <param name="DefaultWebApiEndpointVersion"></param>
public record WebApiEndpointConfiguration(WebApiEndpointVersion DefaultWebApiEndpointVersion)
{
    public WebApiEndpointOpenApiConfiguration OpenApi { get; } = new();

    public WebApiEndpointVersionConfiguration Version { get; } = new();

    public class WebApiEndpointOpenApiConfiguration
    {
        /// <summary>
        /// Default OpenApiInfo for all versions.
        /// <remarks>Unless overridden by <see cref="VersionedInfo"/>.</remarks>
        /// </summary>
        public OpenApiInfo DefaultInfo { get; } = new();

        /// <summary>
        /// OpenApiInfo for a specific version.
        /// <remarks>If version is not specified, <see cref="DefaultInfo"/> is used.</remarks>
        /// </summary>
        public IDictionary<ApiVersion, OpenApiInfo> VersionedInfo { get; } = new Dictionary<ApiVersion, OpenApiInfo>();
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
