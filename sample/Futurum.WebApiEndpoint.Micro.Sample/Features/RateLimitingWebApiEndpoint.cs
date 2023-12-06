namespace Futurum.WebApiEndpoint.Micro.Sample.Features;

[WebApiEndpoint("rate-limiting")]
public partial class RateLimitingWebApiEndpoint
{
    protected override RouteGroupBuilder Configure(RouteGroupBuilder groupBuilder, WebApiEndpointVersion webApiEndpointVersion)
    {
        return groupBuilder.RequireRateLimiting(RateLimiting.SlidingWindow.Policy);
    }

    protected override void Build(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/", GetHandler);
    }

    private static Ok<DataCollectionDto<FeatureDto>> GetHandler(HttpContext context) =>
        Enumerable.Range(0, 10)
                  .Select(i => new Feature($"Name - {i}"))
                  .Select(FeatureMapper.Map)
                  .ToDataCollectionDto()
                  .ToOk();
}
