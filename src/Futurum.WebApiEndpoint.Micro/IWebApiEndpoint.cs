namespace Futurum.WebApiEndpoint.Micro;

public interface IWebApiEndpoint
{
    void Register(IEndpointRouteBuilder builder, WebApiEndpointConfiguration configuration);
}
