namespace Futurum.WebApiEndpoint.Micro;

public interface IWebApiVersionEndpoint
{
    IEndpointRouteBuilder Configure(IEndpointRouteBuilder builder, WebApiEndpointConfiguration configuration);
}
