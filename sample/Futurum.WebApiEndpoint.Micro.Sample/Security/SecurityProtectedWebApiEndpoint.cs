using Futurum.WebApiEndpoint.Micro.Sample.Features;

namespace Futurum.WebApiEndpoint.Micro.Sample.Security;

[WebApiEndpoint("security")]
public class SecurityProtectedWebApiEndpoint : IWebApiEndpoint
{
    public void Configure(RouteGroupBuilder groupBuilder, WebApiEndpointVersion webApiEndpointVersion)
    {
        groupBuilder.RequireAuthorization(Authorization.Permission.Admin);
    }

    public void Register(IEndpointRouteBuilder builder)
    {
        builder.MapGet("protected", ProtectedHandler);
    }

    private static Ok<DataCollectionDto<FeatureDto>> ProtectedHandler(HttpContext context) =>
        Enumerable.Range(0, 10)
                  .Select(i => new Feature($"Name - {i}"))
                  .Select(FeatureMapper.Map)
                  .ToDataCollectionDto()
                  .ToOk();
}