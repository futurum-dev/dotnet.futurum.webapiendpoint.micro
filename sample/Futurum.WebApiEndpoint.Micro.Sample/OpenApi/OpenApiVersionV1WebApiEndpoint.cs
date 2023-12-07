using Futurum.WebApiEndpoint.Micro.Sample.Features;

namespace Futurum.WebApiEndpoint.Micro.Sample.OpenApi;

[WebApiEndpoint("openapi")]
[WebApiEndpointVersion(WebApiEndpointVersions.V1_0.Major, WebApiEndpointVersions.V1_0.Minor)]
[WebApiEndpointVersion(WebApiEndpointVersions.V3_0.Major, WebApiEndpointVersions.V3_0.Minor)]
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
