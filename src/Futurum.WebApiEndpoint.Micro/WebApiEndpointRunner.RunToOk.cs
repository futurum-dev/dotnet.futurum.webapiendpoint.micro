using System.Net;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Futurum.WebApiEndpoint.Micro;

public static partial class WebApiEndpointRunner
{

    public static Results<Ok<T>, BadRequest<ProblemDetails>> RunToOk<T>(
        Func<T> func, HttpContext context, string errorMessage) =>
        Run(func, context, ToOk, errorMessage);


    public static Results<Ok<T>, BadRequest<ProblemDetails>> RunToOk<T>(
        Func<T> func, HttpContext context, Func<string> errorMessage) =>
        Run(func, context, ToOk, errorMessage);

    public static Results<Ok, BadRequest<ProblemDetails>> RunToOk(
        Action func, HttpContext context, string errorMessage)
    {
        try
        {
            func();

            return TypedResults.Ok();
        }
        catch (Exception exception)
        {
            var problemDetails = new ProblemDetails
            {
                Detail = $"{errorMessage};{exception.Message}",
                Instance = context.Request.Path,
                Status = (int)HttpStatusCode.BadRequest,
                Title = ReasonPhrases.GetReasonPhrase((int)HttpStatusCode.BadRequest)
            };

            return TypedResults.BadRequest(problemDetails);
        }
    }

    public static Results<Ok, BadRequest<ProblemDetails>> RunToOk(
        Action func, HttpContext context, Func<string> errorMessage)
    {
        try
        {
            func();

            return TypedResults.Ok();
        }
        catch (Exception exception)
        {
            var problemDetails = new ProblemDetails
            {
                Detail = $"{errorMessage()};{exception.Message}",
                Instance = context.Request.Path,
                Status = (int)HttpStatusCode.BadRequest,
                Title = ReasonPhrases.GetReasonPhrase((int)HttpStatusCode.BadRequest)
            };

            return TypedResults.BadRequest(problemDetails);
        }
    }


    public static Task<Results<Ok<T>, BadRequest<ProblemDetails>>> RunToOkAsync<T>(
        Func<Task<T>> func, HttpContext context, string errorMessage) =>
        RunAsync(func, context, ToOk, errorMessage);


    public static Task<Results<Ok<T>, BadRequest<ProblemDetails>>> RunToOkAsync<T>(
        Func<Task<T>> func, HttpContext context, Func<string> errorMessage) =>
        RunAsync(func, context, ToOk, errorMessage);

    public static async Task<Results<Ok, BadRequest<ProblemDetails>>> RunToOkAsync(
        Func<Task> func, HttpContext context, string errorMessage)
    {
        try
        {
            await func();

            return TypedResults.Ok();
        }
        catch (Exception exception)
        {
            var problemDetails = new ProblemDetails
            {
                Detail = $"{errorMessage};{exception.Message}",
                Instance = context.Request.Path,
                Status = (int)HttpStatusCode.BadRequest,
                Title = ReasonPhrases.GetReasonPhrase((int)HttpStatusCode.BadRequest)
            };

            return TypedResults.BadRequest(problemDetails);
        }
    }

    public static async Task<Results<Ok, BadRequest<ProblemDetails>>> RunToOkAsync(
        Func<Task> func, HttpContext context, Func<string> errorMessage)
    {
        try
        {
            await func();

            return TypedResults.Ok();
        }
        catch (Exception exception)
        {
            var problemDetails = new ProblemDetails
            {
                Detail = $"{errorMessage()};{exception.Message}",
                Instance = context.Request.Path,
                Status = (int)HttpStatusCode.BadRequest,
                Title = ReasonPhrases.GetReasonPhrase((int)HttpStatusCode.BadRequest)
            };

            return TypedResults.BadRequest(problemDetails);
        }
    }
}
