namespace Futurum.WebApiEndpoint.Micro.Sample.Features;

[WebApiEndpoint("rate-limiting")]
public class RateLimitingWebApiEndpoint : IWebApiEndpoint
{
    public void Configure(RouteGroupBuilder groupBuilder, WebApiEndpointVersion webApiEndpointVersion)
    {
        groupBuilder.RequireRateLimiting(RateLimiting.SlidingWindow.PolicyName);
    }

    public void Register(IEndpointRouteBuilder builder)
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