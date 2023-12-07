namespace Futurum.WebApiEndpoint.Micro;

public interface IGlobalWebApiEndpoint
{
    IEndpointRouteBuilder Configure(IEndpointRouteBuilder builder, WebApiEndpointConfiguration configuration);
}