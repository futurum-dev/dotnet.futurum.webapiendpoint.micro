# Futurum.WebApiEndpoint.Micro

![license](https://img.shields.io/github/license/futurum-dev/dotnet.futurum.webapiendpoint.micro?style=for-the-badge)
![CI](https://img.shields.io/github/actions/workflow/status/futurum-dev/dotnet.futurum.webapiendpoint.micro/ci.yml?branch=main&style=for-the-badge)
[![Coverage Status](https://img.shields.io/coveralls/github/futurum-dev/dotnet.futurum.webapiendpoint.micro?style=for-the-badge)](https://coveralls.io/github/futurum-dev/dotnet.futurum.webapiendpoint.micro?branch=main)
[![NuGet version](https://img.shields.io/nuget/v/futurum.webapiendpoint.micro?style=for-the-badge)](https://www.nuget.org/packages/futurum.webapiendpoint.micro)

A dotnet library that allows you to build WebApiEndpoints using a vertical slice architecture approach in a structured way. It's built on top of dotnet 8 and minimal apis.

```csharp
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
```

- [x] Vertical Slice Architecture, gives you the ability to add new features without changing existing code
- [x] Structured way of building WebApiEndpoints using [minimal apis](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-7.0)
- [x] [Easy setup](#easy-setup)
- [x] Full support and built on top of [minimal apis](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-7.0)
- [x] Full support for OpenApi
- [x] Api Versioning baked-in
- [x] Full support for [TypedResults](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.typedresults?view=aspnetcore-7.0)
- [x] Support for configuring
  - [x] [Futurum.WebApiEndpoint.Micro](#configuring-futurumwebapiendpointmicro)
  - [x] [entire API](#configuring-the-entire-api)
  - [x] [specific API version](#configuring-a-specific-api-version)
  - [x] [individual WebApiEndpoint(s)](#webapiendpoint)
  - [x] individual REST method(s) - as per standard minimal apis
- [x] [Supports uploading file(s) with additional JSON payload](#uploading-files-with-additional-json-payload)
- [x] Built in [sandbox runner](#sandbox-runner) with full [TypedResults support](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.typedresults?view=aspnetcore-7.0), catching unhandled exceptions and returning a [ProblemDetails](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.problemdetails?view=aspnetcore-7.0) response
- [x] Autodiscovery of WebApiEndpoint(s), based on Source Generators
- [x] [Roslyn Analysers](#roslyn-analysers) to help build your WebApiEndpoint(s) and ensure best practices
- [x] Built on dotnet 8
- [x] Built in use of [ProblemDetails](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.problemdetails?view=aspnetcore-7.0) support
- [x] Built in [extendable GlobalExceptionHandler](#extendable-globalexceptionhandler)
- [x] Developer friendly, with a simple API and with a full suite of samples and tests
- [x] [Tested solution](https://coveralls.io/github/futurum-dev/dotnet.futurum.webapiendpoint.micro)
- [x] [Comprehensive samples](#comprehensive-samples)
- [x] [Convention Customisation](#convention-customisation)

## What is a WebApiEndpoint?
- It's a vertical slice / feature of your application
- The vertical slice is a self-contained unit of functionality
- Collection of WebApi's that share a route prefix and version. They can also share things like Security, EndpointFilters, RateLimiting, OutputCaching, etc.

## Easy setup
- [x] Add the [NuGet package](https://www.nuget.org/packages/futurum.webapiendpoint.micro) ( futurum.webapiendpoint.micro ) to your project
- [x] Update *program.cs* as per [here](#programcs)

### Example program.cs
```csharp
using Futurum.WebApiEndpoint.Micro;
using Futurum.WebApiEndpoint.Micro.Sample;

var builder = WebApplication.CreateBuilder(args);

builder.Services
       .AddWebApiEndpoints(new WebApiEndpointConfiguration(WebApiEndpointVersions.V1_0))
       .AddWebApiEndpointsForFuturumWebApiEndpointMicroSample();

var app = builder.Build();

app.UseWebApiEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseWebApiEndpointsOpenApi();
}

app.Run();
```

#### AddWebApiEndpointsFor... (per project containing WebApiEndpoints)
This will be automatically created by the source generator. You need to call this to have that project's WebApiEndpoints added to the pipeline.

e.g.
```csharp
builder.Services.AddWebApiEndpointsForFuturumWebApiEndpointMicroSample();
```

#### UseWebApiEndpoints
Adds the WebApiEndpoints to the pipeline
```csharp
app.UseWebApiEndpoints();
```

#### UseWebApiEndpointsOpenApi
Register the OpenApi UI (Swagger and SwaggerUI) middleware. This is usually only done in development mode.
```csharp
app.UseWebApiEndpointsOpenApi();
```

### WebApiEndpoint
1. Create a new partial class
2. Add the *WebApiEndpoint* attribute to the class, with the *route prefix* and optionally a *tag*
3. Add the *WebApiEndpointVersion* attribute to the class, if you want to specify a specific *ApiVersion*
4. Implement the *Build* method and add *minimal api(s)* as per usual
5. *Optionally* implement the *Configure* method to configuration the *WebApiEndpoint*

#### Build
You can *map* your minimal apis for this WebApiEndpoint in the *Build* method.

The *IEndpointRouteBuilder* parameter is already:
- configured with the API versioning
- configured with the route prefix
- been through the *optional* *Configure* method in the same class

```csharp
protected override void Build(IEndpointRouteBuilder builder)
{
}
```

##### Full example
###### Weather
```csharp
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
```

###### File download
```csharp
[WebApiEndpoint("bytes", "feature")]
public partial class BytesWebApiEndpoint
{
    protected override void Build(IEndpointRouteBuilder builder)
    {
        builder.MapGet("download", DownloadHandler);
    }

    private static Results<NotFound, FileContentHttpResult, BadRequest<ProblemDetails>> DownloadHandler(HttpContext context)
    {
        return Run(Execute, context, "Failed to read file");

        Results<NotFound, FileContentHttpResult> Execute()
        {
            var path = "./Data/hello-world.txt";

            if (!File.Exists(path))
            {
                return TypedResults.NotFound();
            }

            var bytes = File.ReadAllBytes(path);
            return TypedResults.Bytes(bytes, MediaTypeNames.Application.Octet, "hello-world.txt");
        }
    }
}
```

### Configure
You can *optionally* configure the WebApiEndpoint in the *Configure* method

```csharp
protected override RouteGroupBuilder Configure(RouteGroupBuilder groupBuilder, WebApiEndpointVersion webApiEndpointVersion)
{
}
```

This allows you to set properties on the RouteGroupBuilder. This will effect all minimal apis in the *Build* method.

You can also configure it differently per ApiVersion.

#### This ia a good place to add a *EndpointFilter*
```csharp
groupBuilder.AddEndpointFilter<CustomEndpointFilter>();
```
#### This ia a good place to add a *RateLimiting*
```csharp
groupBuilder.RequireRateLimiting(RateLimiting.SlidingWindow.Policy);
```
#### This ia a good place to add a *OutputCache*
    
```csharp
groupBuilder.CacheOutput(OutputCaching.ExpiryIn10Seconds.Policy);
```

#### This ia a good place to add *Security*
```csharp
groupBuilder.RequireAuthorization(Authorization.Permission.Admin);
```

## Configure
### Configuring Futurum.WebApiEndpoint.Micro
Allows you to configure:
- DefaultApiVersion *(mandatory)*
  - This is used if a specific ApiVersion is not provided for a specific WebApiEndpoint
- DefaultOpenApiInfo *(optional)*
  - This is used if a specific OpenApiInfo is not provided for a specific ApiVersion
- OpenApiDocumentVersions *(optional)*
  - Allowing you to have different OpenApiInfo per ApiVersion
- VersionPrefix *(optional)*
- VersionFormat *(optional)*
  - uses 'Asp.Versioning.ApiVersionFormatProvider'

#### Example in *program.cs*
```csharp
builder.Services.AddWebApiEndpoints(new WebApiEndpointConfiguration(WebApiEndpointVersions.V1_0)
{
    DefaultOpenApiInfo = new OpenApiInfo
    {
        Title = "Futurum.WebApiEndpoint.Micro.Sample",
    },
    OpenApiDocumentVersions =
    {
        {
            WebApiEndpointVersions.V1_0, 
            new OpenApiInfo
            {
                Title = "Futurum.WebApiEndpoint.Micro.Sample v1"
            }
        }
    }
});
```

### Configuring the entire API
The entire API can be configured. This is a good place to configure things like:
- Global route prefix
- Global authorization (don't forget to set *AllowAnonymous* on the individual WebApiEndpoint that you don't want to be secured i.e. *Login* endpoint)

The class must implement *IGlobalWebApiEndpoint* interface

** NOTE - there can only be one of these classes. **

** NOTE - this is applied before the version route is created. **

#### Example
```csharp
public class GlobalWebApiEndpoint : IGlobalWebApiEndpoint
{
    public IEndpointRouteBuilder Configure(IEndpointRouteBuilder builder, WebApiEndpointConfiguration configuration)
    {
        return builder.MapGroup("api").RequireAuthorization(Authorization.Permission.Admin);
    }
}
```

### Configuring a specific API version
A specific API version can be configured. This is a good place to configure things like:
- API version specific authorization (don't forget to set *AllowAnonymous* on the individual WebApiEndpoint that you don't want to be secured i.e. *Login* endpoint)

The class must:
- implement *IWebApiVersionEndpoint* interface
- be decorated with at least one *WebApiVersionEndpointVersion* attribute

** NOTE - there can only be one of these classes per version. **

** NOTE - this is applied after the version route is created, but before the WebApiEndpoint specific route is created. **

#### Example
```csharp
[WebApiVersionEndpointVersion(WebApiEndpointVersions.V3_0.Major, WebApiEndpointVersions.V3_0.Minor)]
public class WebApiVersionEndpoint3_0 : IWebApiVersionEndpoint
{
    public IEndpointRouteBuilder Configure(IEndpointRouteBuilder builder, WebApiEndpointConfiguration configuration)
    {
        return builder.MapGroup("test-api").RequireAuthorization(Authorization.Permission.Admin);
    }
}
```

## Sandbox runner
### Run and RunAsync - If your code returns an *IResult*
Comprehensive set of extension methods, to run your code in a sandbox
- If your code **does not** throw an unhandled exception, then the existing return remains the same.
- If your code **does** throw an unhandled exception, then a *BadRequest&lt;ProblemDetails&gt;* will be returned, with the appropriate details set on the ProblemDetails.

The returned *Results&lt;...&gt;* type is always augmented to additionally include *BadRequest&lt;ProblemDetails&gt;*

```csharp
TIResult1 -> Results<TIResult1, BadRequest<ProblemDetails>>

Results<TIResult1, TIResult2> -> Results<TIResult1, TIResult2, BadRequest<ProblemDetails>>

Results<TIResult1, TIResult2, TIResult3> -> Results<TIResult1, TIResult2, TIResult3, BadRequest<ProblemDetails>>

Results<TIResult1, TIResult2, TIResult3, TIResult4> -> Results<TIResult1, TIResult2, TIResult3, TIResult4, BadRequest<ProblemDetails>>

Results<TIResult1, TIResult2, TIResult3, TIResult4, TIResult5> -> Results<TIResult1, TIResult2, TIResult3, TIResult4, TIResult5, BadRequest<ProblemDetails>>
```

*Results* has a maximum of 6 types. So 5 are allowed leaving one space left for the *BadRequest&lt;ProblemDetails&gt;*.

#### Example use
```csharp
private static Results<NotFound, FileStreamHttpResult, BadRequest<ProblemDetails>> DownloadHandler(HttpContext context)
{
    return Run(Execute, context, "Failed to read file");

    Results<NotFound, FileStreamHttpResult> Execute()
    {
        var path = "./Data/hello-world.txt";

        if (!File.Exists(path))
        {
            return TypedResults.NotFound();
        }

        var fileStream = File.OpenRead(path);
        return TypedResults.File(fileStream, MediaTypeNames.Application.Octet, "hello-world.txt");
    }
}
```

In this example the *Execute* method is being wrapped by the runner. It returns:
- a *NotFound* if the file does not exist
- a *FileStreamHttpResult* if the file exists

```csharp
Results<NotFound, FileStreamHttpResult>
```

The *Run* / *RunAsync* extension method will change this to add *BadRequest&lt;ProblemDetails&gt;*.

```csharp
Results<NotFound, FileStreamHttpResult, BadRequest<ProblemDetails>>
```

**Note:** It is recommended to add the following to your *GlobalUsings.cs* file.
```csharp
global using static Futurum.WebApiEndpoint.Micro.WebApiEndpointRunner;
```

This means you can use the helper functions without having to specify the namespace. As in the examples.

### RunToOk and RunToOkAsync - If your code returns *void* or  *T* (not a *IResult*)
Comprehensive set of extension methods, to run your code in a sandbox
- If your code **does not** throw an unhandled exception, then the existing return remains the same, *but* will be wrapped in an *Ok*.
- If your code **does** throw an unhandled exception, then a *BadRequest&lt;ProblemDetails&gt;* will be returned, with the appropriate details set on the ProblemDetails.

The returned type from *Run* and *RunAsync* is always augmented to additionally include *BadRequest&lt;ProblemDetails&gt;*

```csharp
void -> Results<Ok, BadRequest<ProblemDetails>>

T -> Results<Ok<T>, BadRequest<ProblemDetails>>
```

#### Example use
```csharp
private static Results<Ok<IAsyncEnumerable<Todo>>, BadRequest<ProblemDetails>> GetAllHandler(HttpContext context, SqliteConnection db)
{
    return RunToOk(Execute, context, "Failed to get todos");

    IAsyncEnumerable<Todo> Execute() =>
        db.QueryAsync<Todo>("SELECT * FROM Todos");
}
```

In this example the *Execute* method returns *IAsyncEnumerable&lt;Todo&gt;*

```csharp
IAsyncEnumerable<Todo>
```

The *RunToOk* / *RunToOkAsync* extension method will
- change the *T* to *Ok&lt;T&gt;*
- add *BadRequest&lt;ProblemDetails&gt;*.

```csharp
Results<Ok<IAsyncEnumerable<Todo>>, BadRequest<ProblemDetails>>
```

**Note:** It is recommended to add the following to your *GlobalUsings.cs* file.
```csharp
global using static Futurum.WebApiEndpoint.Micro.WebApiEndpointRunner;
```

This means you can use the helper functions without having to specify the namespace. As in the examples.

## Uploading file(s) with additional JSON payload
### Upload single file and payload
Use the *FormFileWithPayload* type to upload a single file and a JSON payload

```csharp
private static Task<Results<Ok<FileDetailsWithPayloadDto>, BadRequest<ProblemDetails>>> UploadWithPayloadHandler(HttpContext context, FormFileWithPayload<PayloadDto> fileWithPayload)
{
    return RunAsync(Execute, context, ToOk, "Failed to read file");

    async Task<FileDetailsWithPayloadDto> Execute()
    {
        var tempFile = Path.GetTempFileName();
        await using var stream = File.OpenWrite(tempFile);
        await fileWithPayload.File.CopyToAsync(stream);

        return new FileDetailsWithPayloadDto(fileWithPayload.File.FileName, fileWithPayload.Payload.Name);
    }
}
```

### Upload multiple files and payload
Use the *FormFilesWithPayload* type to upload multiple files and a JSON payload

```csharp
private static Task<Results<Ok<IEnumerable<FileDetailsWithPayloadDto>>, BadRequest<ProblemDetails>>> UploadsWithPayloadHandler(
    HttpContext context, FormFilesWithPayload<PayloadDto> filesWithPayload)
{
    return RunAsync(Execute, context, ToOk, "Failed to read file");

    async Task<IEnumerable<FileDetailsWithPayloadDto>> Execute()
    {
        var fileDetails = new List<FileDetailsWithPayloadDto>();

        foreach (var file in filesWithPayload.Files)
        {
            var tempFile = Path.GetTempFileName();
            await using var stream = File.OpenWrite(tempFile);
            await file.CopyToAsync(stream);

            fileDetails.Add(new FileDetailsWithPayloadDto(file.FileName, filesWithPayload.Payload.Name));
        }

        return fileDetails;
    }
}
```

### Additional helper functions
#### ToOk
Converts a *T* to an *Ok&lt;T&gt;*.

```csharp
ToOk
```

#### ToCreated
Converts a *()* to a *Created*.

```csharp
ToCreated<string>
```

By default it will take the location from the *HttpContext.Request.Path*.

or

Converts a *T* to a *Created&lt;T&gt;*.

This can be overridden by passing in a *string*.

```csharp
ToCreated<T>("/api/articles")
```

#### ToAccepted
Converts a *()* to a *Accepted*.

```csharp
ToAccepted<string>
```

By default it will take the location from the *HttpContext.Request.Path*.

or

Converts a *T* to a *Accepted&lt;T&gt;*.

By default it will take the location from the *HttpContext.Request.Path*.

This can be overridden by passing in a *string*.

```csharp
ToAccepted<T>("/api/articles")
```

## Comprehensive samples
There are examples showing the following:
- [x] A basic blog CRUD implementation
- [x] The *ToDo* sample from Damian Edwards [here](https://github.com/DamianEdwards/TrimmedTodo)
- [x] AsyncEnumerable
- [x] Bytes file download
- [x] EndpointFilter on a specific WebApiEndpoint
- [x] Exception handling
- [x] File(s) upload
- [x] File(s) upload with Payload
- [x] File download
- [x] OpenApi versioning
- [x] Output Caching
- [x] Rate Limiting
- [x] [Security](#security-example) with a basic JWT example on a specific WebApiEndpoint
- [x] Weather Forecast
- [x] Addition project containing WebApiEndpoints
- [x] Configuring setting for entire API versions

![Comprehensive samples](https://raw.githubusercontent.com/futurum-dev/dotnet.futurum.webapiendpoint.micro/main/docs/Futurum.WebApiEndpoint.Micro.Sample-openapi.png)

### Security example
How to use in Swagger UI:
1. Run the Sample project
2. In the Swagger UI, go to the 'Security' 'Login' endpoint
3. Set the following
Username = user1
Password = password1
SetPermissions = true
SetClaim = true
SetRole = true
4. Copy the value returned without double quotes.
5. Go to the 'Security' 'Protected' endpoint
6. Click on the padlock
7. In the value textbox, enter "Bearer " (don't forget the space at the end) + the value returned from the 'Login' endpoint that you copied in step 4.
8. Click "Authorize"
9. Run the 'Protected' endpoint

## Convention Customisation
Although the default conventions are good enough for most cases, you can customise them.

### IWebApiOpenApiVersionConfigurationService
This is used to get the *OpenApiInfo* for each *WebApiEndpointVersion*.

```csharp
serviceCollection.AddWebApiEndpointOpenApiVersionConfigurationService<WebApiOpenApiVersionConfigurationService>();
```

### IWebApiOpenApiVersionUIConfigurationService
This is used to configure the *OpenApi JSON endpoint* for each *WebApiEndpointVersion*.

```csharp
serviceCollection.AddWebApiEndpointOpenApiVersionUIConfigurationService<WebApiOpenApiVersionUIConfigurationService>();
```

### IWebApiVersionConfigurationService
This is used to configure *ApiVersioning* and *ApiExplorer*.

There is an overload of *AddWebApiEndpoints* that takes a generic type of *IWebApiVersionConfigurationService*.
```csharp
builder.Services.AddWebApiEndpoints<CustomWebApiVersionConfigurationService>();
```

Use this instead
```csharp
builder.Services.AddWebApiEndpoints();
```

## Extendable GlobalExceptionHandler
Built in support for handling unhandled exceptions, returning a *ProblemDetails* response.

You can extend the *GlobalExceptionHandler* by adding your own custom exception handling and overriding the default exception handler.

**NOTE: ExceptionToProblemDetailsMapperService is not thread-safe for either:**
- **adding custom exception to ProblemDetails mapping**
- **overriding default exception to ProblemDetails mapping**

**It is recommended to do this in the *program.cs* file.**

### Add to *program.cs*
```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

...
    
var app = builder.Build();

app.UseExceptionHandler();
```

### Add custom Exception to ProblemDetails mapping
In *program.cs* add the following:

```csharp
ExceptionToProblemDetailsMapperService.Add<CustomException>((exception, httpContext, errorMessage) => new()
{
    Detail = "An custom error occurred.",
    Instance = httpContext.Request.Path,
    Status = (int)HttpStatusCode.InternalServerError,
    Title = ReasonPhrases.GetReasonPhrase((int)HttpStatusCode.InternalServerError)
});
```

### Override the Default Exception to ProblemDetails mapping
In *program.cs* add the following:

```csharp
ExceptionToProblemDetailsMapperService.OverrideDefault((exception, httpContext, errorMessage) => new()
{
    Detail = "An error occurred.",
    Instance = httpContext.Request.Path,
    Status = (int)HttpStatusCode.InternalServerError,
    Title = ReasonPhrases.GetReasonPhrase((int)HttpStatusCode.InternalServerError)
});
```

## Roslyn Analysers
- FWAEM0001 - Non empty constructor found on WebApiEndpoint
- FWAEM0002 - BadRequest without 'ProblemDetails' use found on WebApiEndpoint
- FWAEM0003 - Multiple instances found of GlobalWebApiEndpoint
- FWAEM0004 - Multiple instances found of WebApiVersionEndpoint for the same version
