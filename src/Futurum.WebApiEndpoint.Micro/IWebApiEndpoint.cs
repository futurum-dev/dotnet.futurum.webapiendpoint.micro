namespace Futurum.WebApiEndpoint.Micro;

/// <summary>
/// Defines a WebApiEndpoint
/// <list type="bullet">
///     <item>
///         <description>a collection of API endpoints</description>
///     </item>
///     <item>
///         <description>a single group in OpenApi</description>
///     </item>
///     <item>
///         <description>share a route prefix</description>
///     </item>
///     <item>
///         <description>have one or many API versions</description>
///     </item>
/// </list>
/// </summary>
public interface IWebApiEndpoint
{
    /// <summary>
    /// Configure the <see cref="RouteGroupBuilder"/>, can be individually configured for each <see cref="WebApiEndpointVersion"/>
    /// </summary>
    void Configure(RouteGroupBuilder groupBuilder, WebApiEndpointVersion webApiEndpointVersion)
    {
    }

    /// <summary>
    /// Register RouteEndpoint(s) to the IEndpointRouteBuilder
    /// </summary>
    void Register(IEndpointRouteBuilder builder);
}