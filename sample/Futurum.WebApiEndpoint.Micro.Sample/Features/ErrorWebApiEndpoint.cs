namespace Futurum.WebApiEndpoint.Micro.Sample.Features;

[WebApiEndpoint(prefixRoute: "error", group: "feature")]
public class ErrorWebApiEndpoint : IWebApiEndpoint
{
    public void Register(IEndpointRouteBuilder builder)
    {
        builder.MapGet("exception", ExceptionHandler);
    }

    private static Ok ExceptionHandler()
    {
        throw new Exception("We have an Exception!");

        return TypedResults.Ok();
    }
}
