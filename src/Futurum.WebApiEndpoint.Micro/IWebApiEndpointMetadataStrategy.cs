namespace Futurum.WebApiEndpoint.Micro;

public interface IWebApiEndpointMetadataStrategy
{
    IEnumerable<WebApiEndpointMetadata> Get<TWebApiEndpoint>(WebApiEndpointConfiguration configuration, TWebApiEndpoint webApiEndpoint)
        where TWebApiEndpoint : IWebApiEndpoint;
}