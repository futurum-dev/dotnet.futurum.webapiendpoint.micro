using Futurum.Microsoft.Extensions.DependencyInjection;
using Futurum.WebApiEndpoint.Micro;
using Futurum.WebApiEndpoint.Micro.Sample;

using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddProblemDetails();

builder.Services.RegisterModule(new ApplicationModule(builder.Configuration));

builder.AddAuthentication();

builder.Services.AddRateLimiters();
builder.Services.AddOutputCaches();

builder.Services
       .AddWebApiEndpoints(new WebApiEndpointConfiguration(WebApiEndpointVersions.V1_0)
       {
           DefaultOpenApiInfo = new OpenApiInfo
           {
               Title = "Futurum.WebApiEndpoint.Micro.Sample",
           },
           OpenApiDocumentVersions =
           {
               {
                   WebApiEndpointVersions.V3_0,
                   new OpenApiInfo
                   {
                       Title = "Futurum.WebApiEndpoint.Micro.Sample v3"
                   }
               }
           }
       })
       .AddWebApiEndpointsForFuturumWebApiEndpointMicroSampleBlog()
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