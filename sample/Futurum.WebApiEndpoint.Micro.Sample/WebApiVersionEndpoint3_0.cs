namespace Futurum.WebApiEndpoint.Micro.Sample;

[WebApiVersionEndpointVersion(WebApiEndpointVersions.V3_0.Number)]
[WebApiVersionEndpointVersion(WebApiEndpointVersions.V1_20_Beta.Text)]
public class WebApiVersionEndpoint3_0a : IWebApiVersionEndpoint
{
    public RouteGroupBuilder Configure(IEndpointRouteBuilder builder, WebApiEndpointConfiguration configuration)
    {
        return builder.MapGroup("test-api1");
    }
}

// [WebApiVersionEndpointVersion(WebApiEndpointVersions.V3_0.Major, WebApiEndpointVersions.V3_0.Minor)]
// public class WebApiVersionEndpoint3_0b : IWebApiVersionEndpoint
// {
//     public RouteGroupBuilder Configure(IEndpointRouteBuilder builder, WebApiEndpointConfiguration configuration)
//     {
//         return builder.MapGroup("test-api");
//     }
// }
