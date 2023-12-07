using Microsoft.OpenApi.Models;

namespace Futurum.WebApiEndpoint.Micro;

/// <summary>
/// Configuration for WebApiEndpoints
/// </summary>
/// <param name="DefaultWebApiEndpointVersion"></param>
public record WebApiEndpointConfiguration(WebApiEndpointVersion DefaultWebApiEndpointVersion)
{
    /// <summary>
    /// Default OpenApiInfo for all versions. Unless overridden by <see cref="OpenApiDocumentVersions"/>.
    /// </summary>
    public OpenApiInfo? DefaultOpenApiInfo { get; set; }

    /// <summary>
    /// Specific OpenApiInfo for a specific version.
    /// </summary>
    public IDictionary<WebApiEndpointVersion, OpenApiInfo> OpenApiDocumentVersions { get; } = new Dictionary<WebApiEndpointVersion, OpenApiInfo>();

    /// <summary>
    /// Prefix for the versioning
    /// e.g. "v" in "v1.0"
    /// <remarks>Defaults to "v"</remarks>
    /// </summary>
    public string VersionPrefix { get; set; } = "v";

    /// <summary>
    /// See <see cref="Asp.Versioning.ApiVersionFormatProvider"/>.
    /// <remarks>Defaults to "VVV"</remarks>
    /// </summary>
    public string VersionFormat { get; set; } = "VVV";
}
