using Futurum.WebApiEndpoint.Micro.Sample.Features;

namespace Futurum.WebApiEndpoint.Micro.Sample.OpenApi;

[WebApiEndpoint("openapi")]
[WebApiEndpointVersion(WebApiEndpointVersions.V0_1.Major, WebApiEndpointVersions.V0_1.Minor)]
public partial class OpenApiVersionV0WebApiEndpoint
{
    protected override void Build(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/", GetHandler);
    }

    private static Ok<DataCollectionDto<FeatureDto>> GetHandler(HttpContext httpContext, CancellationToken cancellationToken) =>
        Enumerable.Range(0, 10)
                  .Select(i => new Feature($"V0 - Name - {i}"))
                  .Select(FeatureMapper.Map)
                  .ToDataCollectionDto()
                  .ToOk();
}
