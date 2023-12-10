using Futurum.WebApiEndpoint.Micro.Sample.Features;

namespace Futurum.WebApiEndpoint.Micro.Sample.OpenApi;

[WebApiEndpoint("openapi")]
[WebApiEndpointVersion(WebApiEndpointVersions.V1_0.Number)]
[WebApiEndpointVersion(WebApiEndpointVersions.V3_0.Number)]
[WebApiEndpointVersion(WebApiEndpointVersions.V4_0_Alpha.Number, WebApiEndpointVersions.V4_0_Alpha.Status)]
[WebApiEndpointVersion(WebApiEndpointVersions.V1_20_Beta.Text)]
public partial class OpenApiVersionV1WebApiEndpoint
{
    protected override void Build(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/", GetHandler);
    }

    private static Ok<DataCollectionDto<FeatureDto>> GetHandler(HttpContext httpContext, CancellationToken cancellationToken) =>
        Enumerable.Range(0, 10)
                  .Select(i => new Feature($"V1 - Name - {i}"))
                  .Select(FeatureMapper.Map)
                  .ToDataCollectionDto()
                  .ToOk();
}
