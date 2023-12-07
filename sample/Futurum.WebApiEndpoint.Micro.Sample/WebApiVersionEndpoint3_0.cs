using Futurum.WebApiEndpoint.Micro.Sample.Security;

namespace Futurum.WebApiEndpoint.Micro.Sample;

[WebApiVersionEndpointVersion(WebApiEndpointVersions.V3_0.Major, WebApiEndpointVersions.V3_0.Minor)]
public class WebApiVersionEndpoint3_0a : IWebApiVersionEndpoint
{
    public RouteGroupBuilder Configure(IEndpointRouteBuilder builder, WebApiEndpointConfiguration configuration)
    {
        return builder.MapGroup("test-api").RequireAuthorization(Authorization.Permission.Admin);
    }
}

// [WebApiVersionEndpointVersion(WebApiEndpointVersions.V3_0.Major, WebApiEndpointVersions.V3_0.Minor)]
// public class WebApiVersionEndpoint3_0b : IWebApiVersionEndpoint
// {
//     public RouteGroupBuilder Configure(IEndpointRouteBuilder builder, WebApiEndpointConfiguration configuration)
//     {
//         return builder.MapGroup("test-api").RequireAuthorization(Authorization.Permission.Admin);
//     }
// }
