using Futurum.WebApiEndpoint.Micro.Sample.Security;

namespace Futurum.WebApiEndpoint.Micro.Sample;

[GlobalWebApiEndpoint]
public class GlobalWebApiEndpoint : IGlobalWebApiEndpoint
{
    public IEndpointRouteBuilder Configure(IEndpointRouteBuilder builder, WebApiEndpointConfiguration configuration)
    {
        return builder.MapGroup("api");//.RequireAuthorization(Authorization.Permission.Admin);
    }
}
//
// [GlobalWebApiEndpoint]
// public class GlobalWebApiEndpoint2 : IGlobalWebApiEndpoint
// {
//     public IEndpointRouteBuilder Configure(IEndpointRouteBuilder builder, WebApiEndpointConfiguration configuration)
//     {
//         return builder.MapGroup("api").RequireAuthorization(Authorization.Permission.Admin);
//     }
// }
