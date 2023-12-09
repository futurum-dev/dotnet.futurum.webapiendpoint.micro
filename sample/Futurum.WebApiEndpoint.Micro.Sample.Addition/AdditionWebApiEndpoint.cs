using Microsoft.AspNetCore.Http.HttpResults;

namespace Futurum.WebApiEndpoint.Micro.Sample.Addition;

[WebApiEndpoint("addition")]
[WebApiEndpointVersion(2.0d)]
public partial class AdditionWebApiEndpoint
{
    protected override void Build(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/", GetHandler);
    }

    private static Ok<DataCollectionDto<AdditionDto>> GetHandler(HttpContext context) =>
        Enumerable.Range(0, 10)
                  .Select(i => new Addition($"Addition - Name - {i}"))
                  .Select(AdditionMapper.Map)
                  .ToDataCollectionDto()
                  .ToOk();
}
