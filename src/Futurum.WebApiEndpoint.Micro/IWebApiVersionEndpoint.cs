namespace Futurum.WebApiEndpoint.Micro;

public interface IWebApiVersionEndpoint
{
    RouteGroupBuilder Configure(IEndpointRouteBuilder builder, WebApiEndpointConfiguration configuration);
}
