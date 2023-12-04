namespace Futurum.WebApiEndpoint.Micro.Sample.Features;

[WebApiEndpoint("output-caching")]
public partial class OutputCachingWebApiEndpoint
{
    protected override RouteGroupBuilder Configure(RouteGroupBuilder groupBuilder, WebApiEndpointVersion webApiEndpointVersion)
    {
        return groupBuilder.CacheOutput(OutputCaching.ExpiryIn10Seconds.Policy);
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
