namespace Futurum.WebApiEndpoint.Micro.Sample.Addition;

/// <summary>
/// This one will not be used, as when it is registered their will already be one registered for the version.
/// </summary>
[WebApiVersionEndpointVersion(3.0d)]
public class IgnoredWebApiVersionEndpoint3_0 : IWebApiVersionEndpoint
{
    public RouteGroupBuilder Configure(IEndpointRouteBuilder builder, WebApiEndpointConfiguration configuration)
    {
        return builder.MapGroup("test-api2");
    }
}
