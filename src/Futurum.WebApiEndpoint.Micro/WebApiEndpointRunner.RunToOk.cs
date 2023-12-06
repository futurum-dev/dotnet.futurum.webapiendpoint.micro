using System.Net;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Futurum.WebApiEndpoint.Micro;

public static partial class WebApiEndpointRunner
{
    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// If the <paramref name="func"/> is successful, a <see cref="Ok{T}"/> is returned.
    /// If the <paramref name="func"/> throws an exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    /// </summary>
    public static Results<Ok<T>, BadRequest<ProblemDetails>> RunToOk<T>(
        Func<T> func, HttpContext context, string errorMessage)
    {
        try
        {
            var value = func();
            return value.ToOk();
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

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// If the <paramref name="func"/> is successful, a <see cref="Ok{T}"/> is returned.
    /// If the <paramref name="func"/> throws an exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    /// </summary>
    public static Results<Ok<T>, BadRequest<ProblemDetails>> RunToOk<T>(
        Func<T> func, HttpContext context, Func<string> errorMessage)
    {
        try
        {
            var value = func();
            return value.ToOk();
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

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// If the <paramref name="func"/> is successful, a <see cref="Ok"/> is returned.
    /// If the <paramref name="func"/> throws an exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    /// </summary>
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

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// If the <paramref name="func"/> is successful, a <see cref="Ok"/> is returned.
    /// If the <paramref name="func"/> throws an exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    /// </summary>
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

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// If the <paramref name="func"/> is successful, a <see cref="Ok{T}"/> is returned.
    /// If the <paramref name="func"/> throws an exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    /// </summary>
    public static async Task<Results<Ok<T>, BadRequest<ProblemDetails>>> RunToOkAsync<T>(
        Func<Task<T>> func, HttpContext context, string errorMessage)
    {
        try
        {
            var value = await func();
            return value.ToOk();
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

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// If the <paramref name="func"/> is successful, a <see cref="Ok{T}"/> is returned.
    /// If the <paramref name="func"/> throws an exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    /// </summary>
    public static async Task<Results<Ok<T>, BadRequest<ProblemDetails>>> RunToOkAsync<T>(
        Func<Task<T>> func, HttpContext context, Func<string> errorMessage)
    {
        try
        {
            var value = await func();
            return value.ToOk();
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

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// If the <paramref name="func"/> is successful, a <see cref="Ok{T}"/> is returned.
    /// If the <paramref name="func"/> throws an exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    /// </summary>
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

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// If the <paramref name="func"/> is successful, a <see cref="Ok{T}"/> is returned.
    /// If the <paramref name="func"/> throws an exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    /// </summary>
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
