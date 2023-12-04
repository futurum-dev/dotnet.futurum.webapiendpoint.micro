using Futurum.Microsoft.Extensions.DependencyInjection;
using Futurum.WebApiEndpoint.Micro;
using Futurum.WebApiEndpoint.Micro.Sample;

using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddProblemDetails();

builder.Services.AddModule(new ApplicationModule(builder.Configuration));

builder.AddAuthentication();

builder.Services.AddRateLimiters();
builder.Services.AddOutputCaches();

builder.Services
       .AddWebApiEndpoints(new WebApiEndpointConfiguration(new WebApiEndpointVersion(1, 0))
       {
           DefaultOpenApiInfo = new OpenApiInfo
           {
               Title = "Futurum.WebApiEndpoint.Micro.Sample",
           },
           OpenApiDocumentVersions =
           {
               {
                   new WebApiEndpointVersion(3, 0),
                   new OpenApiInfo
                   {
                       Title = "Futurum.WebApiEndpoint.Micro.Sample v3"
                   }
               }
           }
       })
       .AddWebApiEndpointsForFuturumWebApiEndpointMicroSample()
       .AddWebApiEndpointsForFuturumWebApiEndpointMicroSampleAddition();

var app = builder.Build();

app.UseRateLimiter();

app.UseWebApiEndpoints();

app.UseExceptionHandler();
app.UseStatusCodePages();

app.UseHttpsRedirection();

app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.MapGet("/error", () => Results.Problem("An error occurred.", statusCode: 500))
       .ExcludeFromDescription();

    app.UseWebApiEndpointsOpenApi();
}

app.Run();

namespace Futurum.WebApiEndpoint.Micro.Sample
{
    public partial class Program
    {
    }
}
