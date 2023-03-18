using Futurum.WebApiEndpoint.Micro.Sample.Features;

namespace Futurum.WebApiEndpoint.Micro.Sample.OpenApi;

[WebApiEndpoint("openapi")]
[WebApiEndpointVersions.V1_0]
[WebApiEndpointVersions.V3_0]
public class OpenApiVersionV1WebApiEndpoint : IWebApiEndpoint
{
    public void Configure(RouteGroupBuilder groupBuilder, WebApiEndpointVersion webApiEndpointVersion)
    {
    }

    public void Register(IEndpointRouteBuilder builder)
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