namespace Futurum.WebApiEndpoint.Micro.Sample.Addition;

/// <summary>
/// This one will not be used, as when it is registered their will already be one registered.
/// </summary>
public class IgnoredGlobalWebApiEndpoint : IGlobalWebApiEndpoint
{
    public IEndpointRouteBuilder Configure(IEndpointRouteBuilder builder, WebApiEndpointConfiguration configuration)
    {
        return builder.MapGroup("api-2");
    }
}
