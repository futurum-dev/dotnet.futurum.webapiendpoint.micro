namespace Futurum.WebApiEndpoint.Micro.Sample.Features;

[WebApiEndpoint(prefixRoute: "error", group: "feature")]
public class ErrorWebApiEndpoint : IWebApiEndpoint
{
    public void Configure(RouteGroupBuilder groupBuilder, WebApiEndpointVersion webApiEndpointVersion)
    {
    }

    public void Register(IEndpointRouteBuilder builder)
    {
        builder.MapGet("exception", ExceptionHandler);

        builder.MapGet("result-error", ResultErrorHandler);

        builder.MapGet("exception-with-result-error", ExceptionWithResultErrorHandler);
    }

    private static Ok ExceptionHandler()
    {
        throw new Exception("We have an Exception!");
        
        return TypedResults.Ok();
    }

    private static Results<Ok, BadRequest<ProblemDetails>> ResultErrorHandler(HttpContext context) =>
        Result.Fail("We have a ResultError!")
              .ToWebApi(context, ToOk);

    private static Results<Ok, BadRequest<ProblemDetails>> ExceptionWithResultErrorHandler(HttpContext context) =>
        Result.Try(() => throw new Exception("We have an Exception!"), () => "Exception to ResultError")
              .ToWebApi(context, ToOk);
}