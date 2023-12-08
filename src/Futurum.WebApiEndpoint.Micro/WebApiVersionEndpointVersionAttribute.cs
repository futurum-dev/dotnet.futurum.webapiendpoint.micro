namespace Futurum.WebApiEndpoint.Micro;

/// <summary>
/// Configures the WebApi Version for the Endpoints
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class WebApiVersionEndpointVersionAttribute(int majorVersion, int minorVersion) : Attribute
{
    public int MajorVersion { get; } = majorVersion;
    public int MinorVersion { get; } = minorVersion;
}
