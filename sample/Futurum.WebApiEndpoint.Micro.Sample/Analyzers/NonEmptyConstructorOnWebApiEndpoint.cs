namespace Futurum.WebApiEndpoint.Micro.Sample.Analyzers;

[WebApiEndpoint("non-empty-constructor", "analyzer")]
public partial class NonEmptyConstructorOnWebApiEndpoint
{
    public NonEmptyConstructorOnWebApiEndpoint(IConfiguration configuration)
    {
    }

    protected override void Build(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/", GetHandler);
    }

    private static Ok<string> GetHandler(HttpContext context) =>
        "This WebApiEndpoint has a non-empty constructor and will raise a warning.".ToOk();
}
