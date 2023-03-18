namespace Futurum.WebApiEndpoint.Micro;

/// <summary>
/// Configures the PrefixRoute and Group for the WebApiEndpoint
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class WebApiEndpointAttribute : Attribute
{
    public WebApiEndpointAttribute(string? prefixRoute = null)
    {
        PrefixRoute = prefixRoute;
    }
    
    public WebApiEndpointAttribute(string? prefixRoute = null, string? group = null)
    {
        PrefixRoute = prefixRoute;
        Group = group;
    }
    
    public string? PrefixRoute { get; }
    public string? Group { get; }
}