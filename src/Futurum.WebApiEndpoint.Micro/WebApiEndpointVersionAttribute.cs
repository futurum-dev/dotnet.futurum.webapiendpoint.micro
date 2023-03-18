namespace Futurum.WebApiEndpoint.Micro;

/// <summary>
/// Configures the version for the WebApiEndpoint
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class WebApiEndpointVersionAttribute : Attribute
{
    public WebApiEndpointVersionAttribute(int majorVersion)
    {
        MajorVersion = majorVersion;
    }

    public WebApiEndpointVersionAttribute(int majorVersion, int minorVersion)
    {
        MajorVersion = majorVersion;
        MinorVersion = minorVersion;
    }

    public WebApiEndpointVersionAttribute(int majorVersion, int? minorVersion)
    {
        MajorVersion = majorVersion;
        MinorVersion = minorVersion;
    }

    public int MajorVersion { get; }
    public int? MinorVersion { get; }
}