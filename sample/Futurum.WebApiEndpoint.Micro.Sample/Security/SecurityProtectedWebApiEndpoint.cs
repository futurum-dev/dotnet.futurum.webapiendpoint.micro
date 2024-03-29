using Futurum.WebApiEndpoint.Micro.Sample.Features;

namespace Futurum.WebApiEndpoint.Micro.Sample.Security;

[WebApiEndpoint("security")]
public partial class SecurityProtectedWebApiEndpoint
{
    protected override RouteGroupBuilder Configure(RouteGroupBuilder groupBuilder, WebApiEndpointVersion webApiEndpointVersion)
    {
        return groupBuilder.RequireAuthorization(Authorization.Permission.Admin);
    }

    protected override void Build(IEndpointRouteBuilder builder)
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
