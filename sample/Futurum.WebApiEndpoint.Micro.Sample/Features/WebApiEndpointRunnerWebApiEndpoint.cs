namespace Futurum.WebApiEndpoint.Micro.Sample.Features;

[WebApiEndpoint(prefixRoute: "web-api-endpoint-runner")]
public partial class WebApiEndpointRunnerWebApiEndpoint
{
    protected override void Build(IEndpointRouteBuilder builder)
    {
        builder.MapGet("exception", ExceptionHandler);

        builder.MapGet("no-exception", NoExceptionHandler);
    }

    private static Results<Ok, BadRequest<ProblemDetails>> ExceptionHandler(HttpContext context)
    {
        return RunToOk(Execute, context, "Exception caught by WebApiEndpointRunner.");

        void Execute()
        {
            throw new Exception("We have an Exception!");
        }
    }

    private static Results<Ok, BadRequest<ProblemDetails>> NoExceptionHandler(HttpContext context)
    {
        return RunToOk(Execute, context, "We have an Exception!");

        void Execute()
        {
        }
    }
}
