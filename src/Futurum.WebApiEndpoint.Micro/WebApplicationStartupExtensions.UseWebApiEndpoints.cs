namespace Futurum.WebApiEndpoint.Micro;

public static partial class WebApplicationStartupExtensions
{
    /// <summary>
    /// Adds the WebApiEndpoints to the pipeline
    /// </summary>
    public static WebApplication UseWebApiEndpoints(this WebApplication app)
    {
        var webApiEndpoints = app.Services.GetServices<IWebApiEndpoint>();

        var configuration = app.Services.GetService<WebApiEndpointConfiguration>()!;

        foreach (var webApiEndpoint in webApiEndpoints)
        {
            webApiEndpoint.Register(app, configuration);
        }

        return app;
    }
}
