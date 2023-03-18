# Futurum.WebApiEndpoint.Micro

![license](https://img.shields.io/github/license/futurum-dev/dotnet.futurum.webapiendpoint.micro?style=for-the-badge)
![CI](https://img.shields.io/github/actions/workflow/status/futurum-dev/dotnet.futurum.webapiendpoint.micro/ci.yml?branch=main&style=for-the-badge)
[![Coverage Status](https://img.shields.io/coveralls/github/futurum-dev/dotnet.futurum.webapiendpoint.micro?style=for-the-badge)](https://coveralls.io/github/futurum-dev/dotnet.futurum.webapiendpoint.micro?branch=main)
[![NuGet version](https://img.shields.io/nuget/v/futurum.webapiendpoint.micro?style=for-the-badge)](https://www.nuget.org/packages/futurum.webapiendpoint.micro)

A dotnet library that allows you to build WebApiEndpoints using a vertical slice architecture approach. Built on dotnet 7 and minimal apis.

- [x] Vertical Slice Architecture, giving you the ability to add new features without changing existing ones
- [x] Autodiscovery of WebApiEndpoint, based on Source Generators
- [x] Full support and built on top of [minimal apis](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-7.0)
- [x] Full support for OpenApi
- [x] Full support for TypedResults
- [x] Full compatibility with [Futurum.Core](https://www.nuget.org/packages/Futurum.Core)
  - [x] [WebApiResultsExtensions.ToWebApi](#webapiresultsextensionstowebapi)
- [x] Supports uploading file(s) with additional JSON payload
- [x] Api Versioning baked-in
- [x] [Built in Validation support](#validation)
  - [x] [Integrated FluentValidation](#fluentvalidationservice)
  - [x] [Integrated DataAnnotations](#dataannotationsservice)
- [x] [Easy setup](#easy-setup)
- [x] Built on dotnet 7
- [x] Built in use of [ProblemDetails](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.problemdetails?view=aspnetcore-7.0) support
- [x] [Tested solution](https://coveralls.io/github/futurum-dev/dotnet.futurum.webapiendpoint.micro)
- [x] [Comprehensive samples](#comprehensive-samples)

## What is a WebApiEndpoint?
- It's a vertical slice / feature of your application
- The vertical slice is a self-contained unit of functionality
- It's a class that inherits implements IWebApiEndpoint
- They share a route prefix and version

```csharp
[WebApiEndpoint("weather")]
```
- Can have one or many API versions

```csharp
[WebApiEndpointVersion(1)]
```


## Easy setup

### program.cs

#### AddWebApiEndpoints

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

```csharp
builder.Services.AddWebApiEndpoints(new WebApiEndpointConfiguration(WebApiEndpointVersions.V1_0)
{
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

#### AddWebApiEndpointsFor... (per project containing WebApiEndpoints)
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
Register the OpenApi UI (Swagger and SwaggerUI) middleware
```csharp
app.UseWebApiEndpointsOpenApi();
```

#### Full example
```csharp
using Futurum.WebApiEndpoint.Micro;
using Futurum.WebApiEndpoint.Micro.Sample;

using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services
       .AddWebApiEndpoints(new WebApiEndpointConfiguration(WebApiEndpointVersions.V1_0)
       {
           DefaultOpenApiInfo = new OpenApiInfo
           {
               Title = "Futurum.WebApiEndpoint.Micro.Sample",
           }
       })
       .AddWebApiEndpointsForFuturumWebApiEndpointMicroSample();

var app = builder.Build();

app.UseWebApiEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseWebApiEndpointsOpenApi();
}

app.Run();
```

### IWebApiEndpoint

### Configure

You can configure the WebApiEndpoint in the *Configure* method

```csharp
public void Configure(RouteGroupBuilder groupBuilder, WebApiEndpointVersion webApiEndpointVersion)
{
}
```

This allows you to set properties on the RouteGroupBuilder.

You can also configure it differently per ApiVersion.

### Register
You can register the WebApiEndpoint in the *Register* method

```csharp
public void Register(IEndpointRouteBuilder builder)
{
}
```

### Full example
#### Weather
```csharp
[WebApiEndpoint("weather")]
public class WeatherWebApiEndpoint : IWebApiEndpoint
{
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public void Configure(RouteGroupBuilder groupBuilder, WebApiEndpointVersion webApiEndpointVersion)
    {
    }

    public void Register(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/", GetHandler);
    }

    private static Ok<IEnumerable<WeatherForecastDto>> GetHandler(HttpContext httpContext, CancellationToken cancellationToken) =>
        Enumerable.Range(1, 5)
                  .Select(index => new WeatherForecastDto(DateOnly.FromDateTime(DateTime.Now.AddDays(index)), Random.Shared.Next(-20, 55), Summaries[Random.Shared.Next(Summaries.Length)]))
                  .ToOk();
}
```

#### File download
```csharp
[WebApiEndpoint("bytes", "feature")]
public class BytesWebApiEndpoint : IWebApiEndpoint
{
    public void Configure(RouteGroupBuilder groupBuilder, WebApiEndpointVersion webApiEndpointVersion)
    {
    }

    public void Register(IEndpointRouteBuilder builder)
    {
        builder.MapGet("download", DownloadHandler);
    }

    private static Results<NotFound, FileContentHttpResult, BadRequest<ProblemDetails>> DownloadHandler(HttpContext context)
    {
        return Result.Try(Execute, () => "Failed to read file")
                     .ToWebApi(context);

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

## Comprehensive samples
There are examples showing the following:
- [x] A basic blog CRUD implementation
- [x] The *ToDo* sample from Damian Edwards [here](https://github.com/DamianEdwards/TrimmedTodo)
- [x] AsyncEnumerable
- [x] Bytes file download
- [x] EndpointFilter on a specific WebApiEndpoint
- [x] Exception handling
- [x] [Result](https://docs.futurum.dev/dotnet.futurum.core/result/overview.html) error handling
- [x] File(s) upload
- [x] File(s) upload with Payload
- [x] File download
- [x] OpenApi version support
- [x] Security with a basic JWT example on a specific WebApiEndpoint
- [x] Validation - DataAnnotations and FluentValidation and both combined
- [x] Weather Forecast
- [x] Addition project containing WebApiEndpoints

![Futurum.WebApiEndpoint.Micro.Sample-openapi.png](docs%2FFuturum.WebApiEndpoint.Micro.Sample-openapi.png)

## Validation
### ValidationService
Executes FluentValidation and DataAnnotations
```csharp
IValidationService<ArticleDto> validationService
```

```csharp
private static Results<Ok<ArticleDto>, ValidationProblem, BadRequest<ProblemDetails>> ValidationHandler(HttpContext context, IValidationService<ArticleDto> validationService,
                                                                                                        ArticleDto articleDto) =>
    validationService.Execute(articleDto)
                     .Map(() => new Article(null, articleDto.Url))
                     .Map(ArticleMapper.MapToDto)
                     .ToWebApi(context, ToOk, ToValidationProblem);
```

### FluentValidationService
Calls FluentValidation
```csharp
IFluentValidationService<ArticleDto> fluentValidationService
```

e.g.
```csharp
private static Results<Ok<ArticleDto>, ValidationProblem, BadRequest<ProblemDetails>> FluentValidationHandler(HttpContext context, IFluentValidationService<ArticleDto> fluentValidationService,
                                                                                                              ArticleDto articleDto) =>
    fluentValidationService.Execute(articleDto)
                           .Map(() => new Article(null, articleDto.Url))
                           .Map(ArticleMapper.MapToDto)
                           .ToWebApi(context, ToOk, ToValidationProblem);

public class ArticleDtoValidator : AbstractValidator<ArticleDto>
{
    public ArticleDtoValidator()
    {
        RuleFor(x => x.Url).NotEmpty().WithMessage("must have a value;");
    }
}
```

### DataAnnotationsService

```csharp
IDataAnnotationsValidationService dataAnnotationsValidationService
```

```csharp
private static Results<Ok<ArticleDto>, ValidationProblem, BadRequest<ProblemDetails>> DataAnnotationsValidationHandler(HttpContext context,
                                                                                                                       IDataAnnotationsValidationService dataAnnotationsValidationService,
                                                                                                                       ArticleDto articleDto) =>
    dataAnnotationsValidationService.Execute(articleDto)
                                    .Map(() => new Article(null, articleDto.Url))
                                    .Map(ArticleMapper.MapToDto)
                                    .ToWebApi(context, ToOk, ToValidationProblem);
```

## Full compatibility with [Futurum.Core](https://www.nuget.org/packages/Futurum.Core)

### WebApiResultsExtensions.ToWebApi

Comprehensive set of extension methods to transform a [Result](https://docs.futurum.dev/dotnet.futurum.core/result/overview.html) and [Result&lt;T&gt;](https://docs.futurum.dev/dotnet.futurum.core/result/overview.html) to an *TypedResult*.

#### Result&lt;IResult&gt; -> Results&lt;IResult, BadRequest&lt;ProblemDetails&gt;&gt;
- If the Result&lt;IResult&gt; is a *Success&lt;IResult&gt;* then the *IResult* will be returned.
- If the Result&lt;T&gt; is a *Failure&lt;T&gt;* then the *BadRequest&lt;ProblemDetails&gt;* will be returned, with the appropriate details set on the ProblemDetails. The *error message* will be safe to return to the client, that is, it will not contain any sensitive information e.g. StackTrace.

This works for:

```csharp
Result<IResult>

Result<Results<IResult, IResult>>

Result<Results<IResult, IResult, IResult>>

Result<Results<IResult, IResult, IResult, IResult>>

Result<Results<IResult, IResult, IResult, IResult, IResult>>

```
*Results* has a maximum of 6 types. So 5 are allowed leaving one space left for the *BadRequest&lt;ProblemDetails&gt;*.

##### Example use
In this example the *Execute* method will return:
- a *NotFound* if the file does not exist
- a *FileStreamHttpResult* if the file exists

```csharp
Results<NotFound, FileStreamHttpResult>
```

The *ToWebApi* extension method will change this to add *BadRequest&lt;ProblemDetails&gt;*.

```csharp
Results<NotFound, FileStreamHttpResult, BadRequest<ProblemDetails>>
```

#### Full Example
```csharp
private static Results<NotFound, FileStreamHttpResult, BadRequest<ProblemDetails>> DownloadHandler(HttpContext context)
{
    return Result.Try(Execute, () => "Failed to read file")
                 .ToWebApi(context);

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

#### How to handle *successful* and *failure* cases in a typed way with *TypedResult*
You can optionally specify which TypedResult success cases you want to handle. This is useful if you want to handle a specific successes case differently.

You can specify which TypedResult error cases you want to handle. This is useful if you want to handle a specific error case differently.

If you have a *success* case, you must pass in the the *success* helper function first, then the *failure* helper functions.

There can only be 1 *success* helper function, but there can be multiple *failure* helper functions.

##### Example use
The *ToWebApi* extension method will change the method return type to add *BadRequest&lt;ProblemDetails&gt;*, with the appropriate details set on the ProblemDetails. The *error message* will be safe to return to the client, that is, it will not contain any sensitive information e.g. StackTrace.

You can then pass in additional helper functions to deal with successes and failures and these will change the return type to the appropriate *TypedResult*'s.

*ToOk* is a function that will convert a *T* to an *Ok&lt;T&gt;*.

*ToValidationProblem* is a function that will convert a *ValidationResultError* to a *ValidationProblem*.

#### Full Example
```csharp
private static Results<Ok<ArticleDto>, ValidationProblem, BadRequest<ProblemDetails>> ValidationHandler(HttpContext context, IValidationService<ArticleDto> validationService,
                                                                                                        ArticleDto articleDto) =>
    validationService.Execute(articleDto)
                     .Map(() => new Article(null, articleDto.Url))
                     .Map(ArticleMapper.MapToDto)
                     .ToWebApi(context, ToOk, ToValidationProblem);
```

### Success and Failure helper functions
If you have a *success* case, you must pass in the the *success* helper function first, then the *failure* helper functions.

There can only be 1 *success* helper function, but there can be multiple *failure* helper functions.

**Note:** It is recommended to add the following to your *GlobalUsings.cs* file.
```csharp
global using static Futurum.WebApiEndpoint.Micro.WebApiResultsExtensions;
```
This means you can use the helper functions without having to specify the namespace. As in the examples.

#### Success
##### ToOk
Converts a *T* to an *Ok&lt;T&gt;*.

```csharp
ToOk
```

##### ToCreated
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

##### ToAccepted
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

#### Failure
##### ToNotFound
If a *ResultErrorKeyNotFound* has occured then it will convert it to a *NotFound&lt;ProblemDetails&gt;*, with the correct information set on the *ProblemDetails*.

```csharp
ToNotFound
```

##### ToValidationProblem
If a *ResultErrorValidation* has occured then it will convert it to a *ValidationProblem*, with the correct information set on the *HttpValidationProblemDetails*.

```csharp
ToValidationProblem
```