namespace Futurum.WebApiEndpoint.Micro;

/// <summary>
/// Global configuration the WebApiEndpoints
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class GlobalWebApiEndpointAttribute : Attribute
{
    public GlobalWebApiEndpointAttribute()
    {
    }
}