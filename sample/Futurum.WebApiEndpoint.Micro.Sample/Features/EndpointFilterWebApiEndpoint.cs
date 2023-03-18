namespace Futurum.WebApiEndpoint.Micro.Sample.Features;

[WebApiEndpoint("endpoint-filter", "feature")]
public class EndpointFilterWebApiEndpoint : IWebApiEndpoint
{
    public void Configure(RouteGroupBuilder groupBuilder, WebApiEndpointVersion webApiEndpointVersion)
    {
        groupBuilder.AddEndpointFilter<CustomEndpointFilter>();
    }

    public void Register(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/{id}", GetHandler);
    }

    private static Ok<DataCollectionDto<FeatureDto>> GetHandler(HttpContext context, ILogger<EndpointFilterWebApiEndpoint> logger, string id)
    {
        logger.LogInformation("GetHandler start using parameter id: {Id}", id);

        var result = Enumerable.Range(0, 10)
                                 .Select(i => new Feature($"Name - {i}"))
                                 .Select(FeatureMapper.Map)
                                 .ToDataCollectionDto()
                                 .ToOk();
        
        logger.LogInformation("GetHandler finished using parameter id: {Id}", id);
        
        return result;
    }

    public class CustomEndpointFilter : IEndpointFilter
    {
        private readonly ILogger _logger;

        public CustomEndpointFilter(ILogger<CustomEndpointFilter> logger)
        {
            _logger = logger;
        }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var id = context.HttpContext.GetRouteData().Values["id"];
            
            _logger.LogInformation("AddEndpointFilter 'CustomEndpointFilter' before filter using parameter id: {Id}", id);
            
            var result = await next(context);
            
            _logger.LogInformation("AddEndpointFilter 'CustomEndpointFilter' after filter using parameter id: {Id}", id);
            
            return result;
        }
    }
}