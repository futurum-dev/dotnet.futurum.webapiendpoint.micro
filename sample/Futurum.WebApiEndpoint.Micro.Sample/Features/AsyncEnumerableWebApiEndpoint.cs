namespace Futurum.WebApiEndpoint.Micro.Sample.Features;

[WebApiEndpoint("async-enumerable", "feature")]
public class AsyncEnumerableWebApiEndpoint : IWebApiEndpoint
{
    public void Register(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/", GetHandler);
    }

    private static Ok<IAsyncEnumerable<FeatureDto>> GetHandler(HttpContext context) =>
        AsyncEnumerable.Range(0, 10)
                       .Select(i => new Feature($"Name - {i}"))
                       .Select(FeatureMapper.Map)
                       .ToOk();
}