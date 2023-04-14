namespace Futurum.WebApiEndpoint.Micro.Sample.Analyzers;

[WebApiEndpoint(prefixRoute: "method-returning-BadRequest-without-ProblemDetails", group: "analyzer")]
public class MethodReturningBadRequestWithoutProblemDetails : IWebApiEndpoint
{
    public void Register(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/", ResultErrorHandler);
    }
    
    private static Results<Ok<string>, BadRequest<string>> ResultErrorHandler(HttpContext context) =>
        "This WebApiEndpoint has a non-empty constructor and will raise a warning.".ToOk();
}