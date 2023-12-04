namespace Futurum.WebApiEndpoint.Micro.Sample.WeatherForecast;

[WebApiEndpoint("weather")]
public partial class WeatherWebApiEndpoint
{
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    protected override void Build(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/", GetHandler);
    }

    private static Ok<IEnumerable<WeatherForecastDto>> GetHandler(HttpContext httpContext, CancellationToken cancellationToken) =>
        Enumerable.Range(1, 5)
                  .Select(index => new WeatherForecastDto(DateOnly.FromDateTime(DateTime.Now.AddDays(index)), Random.Shared.Next(-20, 55), Summaries[Random.Shared.Next(Summaries.Length)]))
                  .ToOk();
}
