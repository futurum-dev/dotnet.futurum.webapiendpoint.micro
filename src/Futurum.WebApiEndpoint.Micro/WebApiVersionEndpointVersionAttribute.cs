using Asp.Versioning;

namespace Futurum.WebApiEndpoint.Micro;

/// <summary>
/// Configures the WebApi Version for the Endpoints
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class WebApiVersionEndpointVersionAttribute : Attribute
{
    public WebApiVersionEndpointVersionAttribute(double version, string? status = default)
    {
        ApiVersion = new(version, status);
    }

    public WebApiVersionEndpointVersionAttribute(string version)
    {
        ApiVersion = ApiVersionParser.Default.Parse(version);
    }

    public ApiVersion ApiVersion { get; }
}
