using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;

namespace Futurum.WebApiEndpoint.Micro;

public class WebApiEndpointOpenApiInfo
{
    /// <summary>
    /// REQUIRED. The title of the application.
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// A short description of the application.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// A URL to the Terms of Service for the API. MUST be in the format of a URL.
    /// </summary>
    public Uri? TermsOfService { get; set; }

    /// <summary>
    /// The contact information for the exposed API.
    /// </summary>
    public OpenApiContact? Contact { get; set; }

    /// <summary>
    /// The license information for the exposed API.
    /// </summary>
    public OpenApiLicense? License { get; set; }

    /// <summary>
    /// This object MAY be extended with Specification Extensions.
    /// </summary>
    public IDictionary<string, IOpenApiExtension> Extensions { get; } = new Dictionary<string, IOpenApiExtension>();
}
