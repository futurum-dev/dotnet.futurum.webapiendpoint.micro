using Asp.Versioning;

namespace Futurum.WebApiEndpoint.Micro;

/// <summary>
/// Configures the version for the WebApiEndpoint
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class WebApiEndpointVersionAttribute : Attribute
{
    public WebApiEndpointVersionAttribute(double version, string? status = default)
    {
        ApiVersion = new ApiVersion(version, status);
    }

    public WebApiEndpointVersionAttribute(string version)
    {
        ApiVersion = ApiVersionParser.Default.Parse(version);
    }

    public ApiVersion ApiVersion { get; }
}
