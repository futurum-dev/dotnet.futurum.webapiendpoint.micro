using Futurum.WebApiEndpoint.Micro.Sample.Features;

namespace Futurum.WebApiEndpoint.Micro.Sample.OpenApi;

[WebApiEndpoint("openapi")]
[WebApiEndpointVersion(2, 0)]
public partial class OpenApiVersionV2WebApiEndpoint
{
    protected override void Build(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/", GetHandler);
    }

    private static Ok<DataCollectionDto<FeatureDto>> GetHandler(HttpContext httpContext, CancellationToken cancellationToken) =>
        Enumerable.Range(0, 10)
                  .Select(i => new Feature($"V2 - Name - {i}"))
                  .Select(FeatureMapper.Map)
                  .ToDataCollectionDto()
                  .ToOk();
}
