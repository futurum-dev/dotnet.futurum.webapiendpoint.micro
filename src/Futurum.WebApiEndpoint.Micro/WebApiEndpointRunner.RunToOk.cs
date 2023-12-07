using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.Micro;

public static partial class WebApiEndpointRunner
{
    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> is successful, a <see cref="Ok{T}"/> is returned.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> throws an unhandled exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    ///         </description>
    ///     </item>
    /// </list>
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
            var problemDetails = ExceptionToProblemDetailsMapperService.Instance.Map(exception, context, errorMessage);

            return TypedResults.BadRequest(problemDetails);
        }
    }

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> is successful, a <see cref="Ok{T}"/> is returned.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> throws an unhandled exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    ///         </description>
    ///     </item>
    /// </list>
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
            var problemDetails = ExceptionToProblemDetailsMapperService.Instance.Map(exception, context, errorMessage());

            return TypedResults.BadRequest(problemDetails);
        }
    }

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> is successful, a <see cref="Ok"/> is returned.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> throws an unhandled exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    ///         </description>
    ///     </item>
    /// </list>
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
            var problemDetails = ExceptionToProblemDetailsMapperService.Instance.Map(exception, context, errorMessage);

            return TypedResults.BadRequest(problemDetails);
        }
    }

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> is successful, a <see cref="Ok"/> is returned.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> throws an unhandled exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    ///         </description>
    ///     </item>
    /// </list>
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
            var problemDetails = ExceptionToProblemDetailsMapperService.Instance.Map(exception, context, errorMessage());

            return TypedResults.BadRequest(problemDetails);
        }
    }

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> is successful, a <see cref="Ok{T}"/> is returned.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> throws an unhandled exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    ///         </description>
    ///     </item>
    /// </list>
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
            var problemDetails = ExceptionToProblemDetailsMapperService.Instance.Map(exception, context, errorMessage);

            return TypedResults.BadRequest(problemDetails);
        }
    }

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> is successful, a <see cref="Ok{T}"/> is returned.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> throws an unhandled exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    ///         </description>
    ///     </item>
    /// </list>
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
            var problemDetails = ExceptionToProblemDetailsMapperService.Instance.Map(exception, context, errorMessage());

            return TypedResults.BadRequest(problemDetails);
        }
    }

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> is successful, a <see cref="Ok"/> is returned.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> throws an unhandled exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    ///         </description>
    ///     </item>
    /// </list>
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
            var problemDetails = ExceptionToProblemDetailsMapperService.Instance.Map(exception, context, errorMessage);

            return TypedResults.BadRequest(problemDetails);
        }
    }

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> is successful, a <see cref="Ok"/> is returned.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> throws an unhandled exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    ///         </description>
    ///     </item>
    /// </list>
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
            var problemDetails = ExceptionToProblemDetailsMapperService.Instance.Map(exception, context, errorMessage());

            return TypedResults.BadRequest(problemDetails);
        }
    }
}
