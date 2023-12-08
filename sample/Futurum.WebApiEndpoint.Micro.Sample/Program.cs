using Futurum.Microsoft.Extensions.DependencyInjection;
using Futurum.WebApiEndpoint.Micro;
using Futurum.WebApiEndpoint.Micro.Sample;

using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddModule(new ApplicationModule(builder.Configuration));

builder.AddAuthentication();

builder.Services.AddRateLimiters();

builder.Services.AddOutputCaches();

builder.Services
       .AddWebApiEndpoints(new WebApiEndpointConfiguration(WebApiEndpointVersions.V1_0.Version)
       {
           OpenApi =
           {
               DefaultInfo =
               {
                   Title = "Futurum.WebApiEndpoint.Micro.Sample",
               },
               VersionedInfo =
               {
                   {
                       WebApiEndpointVersions.V3_0.Version,
                       new OpenApiInfo
                       {
                           Title = "Futurum.WebApiEndpoint.Micro.Sample v3"
                       }
                   }
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
