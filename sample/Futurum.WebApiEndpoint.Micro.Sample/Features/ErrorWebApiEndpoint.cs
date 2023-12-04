namespace Futurum.WebApiEndpoint.Micro.Sample.Features;

[WebApiEndpoint(prefixRoute: "error", group: "feature")]
public partial class ErrorWebApiEndpoint
{
    protected override void Build(IEndpointRouteBuilder builder)
    {
        builder.MapGet("exception", ExceptionHandler);
    }

    private static Ok ExceptionHandler()
    {
        throw new Exception("We have an Exception!");

        return TypedResults.Ok();
    }
}
