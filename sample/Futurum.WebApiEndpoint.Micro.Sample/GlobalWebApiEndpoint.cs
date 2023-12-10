namespace Futurum.WebApiEndpoint.Micro.Sample;

public class GlobalWebApiEndpoint : IGlobalWebApiEndpoint
{
    public IEndpointRouteBuilder Configure(IEndpointRouteBuilder builder, WebApiEndpointConfiguration configuration)
    {
        return builder.MapGroup("api");//.RequireAuthorization(Authorization.Permission.Admin);
    }
}
//
// public class GlobalWebApiEndpoint2 : IGlobalWebApiEndpoint
// {
//     public IEndpointRouteBuilder Configure(IEndpointRouteBuilder builder, WebApiEndpointConfiguration configuration)
//     {
//         return builder.MapGroup("api").RequireAuthorization(Authorization.Permission.Admin);
//     }
// }
