namespace Futurum.WebApiEndpoint.Micro.Sample.Analyzers;

[WebApiEndpoint("non-empty-constructor", "analyzer")]
public class NonEmptyConstructorOnWebApiEndpoint : IWebApiEndpoint
{
    public NonEmptyConstructorOnWebApiEndpoint(IConfiguration configuration)
    {
    }
    
    public void Register(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/", GetHandler);
    }

    private static Ok<string> GetHandler(HttpContext context) =>
        "This WebApiEndpoint has a non-empty constructor and will raise a warning.".ToOk();
}