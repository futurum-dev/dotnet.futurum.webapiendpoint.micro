using Futurum.Microsoft.Extensions.DependencyInjection;
using Futurum.WebApiEndpoint.Micro;
using Futurum.WebApiEndpoint.Micro.Sample;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddModule(new ApplicationModule(builder.Configuration));

builder.AddAuthentication();

builder.Services.AddRateLimiters();

builder.Services.AddOutputCaches();

builder.Services
       .AddWebApiEndpoints(new WebApiEndpointConfiguration
       {
           DefaultApiVersion = WebApiEndpointVersions.V1_0.Version,
           OpenApi = new()
           {
               DefaultInfo = new()
               {
                   Title = "Futurum.WebApiEndpoint.Micro.Sample",
                   Description = "OpenAPI description default",
                   Contact = new()
                   {
                       Email = "a@b.com",
                       Name = "A B",
                       Url = new Uri("https://www.google.com")
                   },
                   License = new()
                   {
                       Name = "MIT"
                   },
                   TermsOfService = new Uri("https://www.google.com")
               },
               VersionedOverrideInfo =
               {
                   {
                       WebApiEndpointVersions.V3_0.Version,
                       new WebApiEndpointOpenApiInfo
                       {
                           Title = "Futurum.WebApiEndpoint.Micro.Sample v3",
                           Description = "OpenAPI description v3",
                           TermsOfService = new Uri("https://www.bing.com")
                       }
                   },
               }
           }
       })
       .AddWebApiEndpointsForFuturumWebApiEndpointMicroSample()
       .AddWebApiEndpointsForFuturumWebApiEndpointMicroSampleAddition();

var app = builder.Build();

app.UseExceptionHandler();

app.UseRateLimiter();

app.UseWebApiEndpoints();

app.UseStatusCodePages();

app.UseHttpsRedirection();

app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseWebApiEndpointsOpenApi();
}

app.Run();

namespace Futurum.WebApiEndpoint.Micro.Sample
{
    public partial class Program
    {
    }
}
