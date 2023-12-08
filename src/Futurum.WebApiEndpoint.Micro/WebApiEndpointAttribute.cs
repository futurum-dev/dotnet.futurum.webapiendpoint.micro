namespace Futurum.WebApiEndpoint.Micro;

/// <summary>
/// Configures the PrefixRoute and Group for the WebApiEndpoint
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class WebApiEndpointAttribute(string? prefixRoute = null, string? group = null) : Attribute
{
    public string? PrefixRoute { get; } = prefixRoute;
    public string? Group { get; } = group;
}
