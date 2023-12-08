namespace Futurum.WebApiEndpoint.Micro;

/// <summary>
/// Configures the version for the WebApiEndpoint
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class WebApiEndpointVersionAttribute(int majorVersion, int minorVersion) : Attribute
{
    public int MajorVersion { get; } = majorVersion;
    public int MinorVersion { get; } = minorVersion;
}
