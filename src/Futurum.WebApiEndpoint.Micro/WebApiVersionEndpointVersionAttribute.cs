namespace Futurum.WebApiEndpoint.Micro;

/// <summary>
/// Configures the WebApi Version for the Endpoints
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class WebApiVersionEndpointVersionAttribute : Attribute
{
    public WebApiVersionEndpointVersionAttribute(int majorVersion, int minorVersion)
    {
        MajorVersion = majorVersion;
        MinorVersion = minorVersion;
    }

    public int MajorVersion { get; }
    public int MinorVersion { get; }
}