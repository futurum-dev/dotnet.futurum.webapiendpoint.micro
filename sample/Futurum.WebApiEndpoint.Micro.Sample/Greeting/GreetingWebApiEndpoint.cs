namespace Futurum.WebApiEndpoint.Micro.Sample.Greeting;

[WebApiEndpoint("greeting")]
public partial class GreetingWebApiEndpoint
{
    protected override void Build(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/hello", HelloHandler);
        builder.MapGet("/goodbye", GoodbyeHandler);
    }

    private static Ok<string> HelloHandler(HttpContext context, string name) =>
        $"Hello {name}".ToOk();

    private static Ok<string> GoodbyeHandler(HttpContext context, string name) =>
        $"Goodbye {name}".ToOk();
}
