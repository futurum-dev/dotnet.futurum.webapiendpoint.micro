using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.Micro;

public static partial class WebApiEndpointRunner
{
    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> is successful, it is returned.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> throws an unhandled exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    ///         </description>
    ///     </item>
    /// </list>
    /// </summary>
    public static Results<TIResult1, BadRequest<ProblemDetails>> Run<TIResult1>(
        Func<TIResult1> func, HttpContext context, string errorMessage)
        where TIResult1 : IResult
    {
        try
        {
            var result = func();
            return result;
        }
        catch (Exception exception)
        {
            var problemDetails = exception.ToProblemDetails(context, errorMessage);

            return TypedResults.BadRequest(problemDetails);
        }
    }

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> is successful, it is returned.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> throws an unhandled exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    ///         </description>
    ///     </item>
    /// </list>
    /// </summary>
    public static Results<TIResult1, BadRequest<ProblemDetails>> Run<TIResult1>(
        Func<TIResult1> func, HttpContext context, Func<string> errorMessage)
        where TIResult1 : IResult
    {
        try
        {
            var result = func();
            return result;
        }
        catch (Exception exception)
        {
            var problemDetails = exception.ToProblemDetails(context, errorMessage());

            return TypedResults.BadRequest(problemDetails);
        }
    }

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> is successful, it is returned.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> throws an unhandled exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    ///         </description>
    ///     </item>
    /// </list>
    /// </summary>
    public static Results<TIResult1, TIResult2, BadRequest<ProblemDetails>> Run<TIResult1, TIResult2>(
        Func<Results<TIResult1, TIResult2>> func, HttpContext context, string errorMessage)
        where TIResult1 : IResult
        where TIResult2 : IResult
    {
        try
        {
            var results = func();
            return results.Result switch
            {
                TIResult1 result1 => result1,
                TIResult2 result2 => result2,
                _                 => throw new InvalidOperationException($"Unexpected result type.")
            };
        }
        catch (Exception exception)
        {
            var problemDetails = exception.ToProblemDetails(context, errorMessage);

            return TypedResults.BadRequest(problemDetails);
        }
    }

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> is successful, it is returned.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> throws an unhandled exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    ///         </description>
    ///     </item>
    /// </list>
    /// </summary>
    public static Results<TIResult1, TIResult2, BadRequest<ProblemDetails>> Run<TIResult1, TIResult2>(
        Func<Results<TIResult1, TIResult2>> func, HttpContext context, Func<string> errorMessage)
        where TIResult1 : IResult
        where TIResult2 : IResult
    {
        try
        {
            var results = func();
            return results.Result switch
            {
                TIResult1 result1 => result1,
                TIResult2 result2 => result2,
                _                 => throw new InvalidOperationException($"Unexpected result type.")
            };
        }
        catch (Exception exception)
        {
            var problemDetails = exception.ToProblemDetails(context, errorMessage());

            return TypedResults.BadRequest(problemDetails);
        }
    }

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> is successful, it is returned.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> throws an unhandled exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    ///         </description>
    ///     </item>
    /// </list>
    /// </summary>
    public static Results<TIResult1, TIResult2, TIResult3, BadRequest<ProblemDetails>> Run<TIResult1, TIResult2, TIResult3>(
        Func<Results<TIResult1, TIResult2, TIResult3>> func, HttpContext context, string errorMessage)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TIResult3 : IResult
    {
        try
        {
            var results = func();
            return results.Result switch
            {
                TIResult1 result1 => result1,
                TIResult2 result2 => result2,
                TIResult3 result3 => result3,
                _                 => throw new InvalidOperationException("Unexpected result type.")
            };
        }
        catch (Exception exception)
        {
            var problemDetails = exception.ToProblemDetails(context, errorMessage);

            return TypedResults.BadRequest(problemDetails);
        }
    }

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> is successful, it is returned.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> throws an unhandled exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    ///         </description>
    ///     </item>
    /// </list>
    /// </summary>
    public static Results<TIResult1, TIResult2, TIResult3, BadRequest<ProblemDetails>> Run<TIResult1, TIResult2, TIResult3>(
        Func<Results<TIResult1, TIResult2, TIResult3>> func, HttpContext context, Func<string> errorMessage)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TIResult3 : IResult
    {
        try
        {
            var results = func();
            return results.Result switch
            {
                TIResult1 result1 => result1,
                TIResult2 result2 => result2,
                TIResult3 result3 => result3,
                _                 => throw new InvalidOperationException("Unexpected result type.")
            };
        }
        catch (Exception exception)
        {
            var problemDetails = exception.ToProblemDetails(context, errorMessage());

            return TypedResults.BadRequest(problemDetails);
        }
    }

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> is successful, it is returned.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> throws an unhandled exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    ///         </description>
    ///     </item>
    /// </list>
    /// </summary>
    public static Results<TIResult1, TIResult2, TIResult3, TIResult4, BadRequest<ProblemDetails>> Run<TIResult1, TIResult2, TIResult3, TIResult4>(
        Func<Results<TIResult1, TIResult2, TIResult3, TIResult4>> func, HttpContext context, string errorMessage)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TIResult3 : IResult
        where TIResult4 : IResult
    {
        try
        {
            var results = func();
            return results.Result switch
            {
                TIResult1 result1 => result1,
                TIResult2 result2 => result2,
                TIResult3 result3 => result3,
                TIResult4 result4 => result4,
                _                 => throw new InvalidOperationException("Unexpected result type.")
            };
        }
        catch (Exception exception)
        {
            var problemDetails = exception.ToProblemDetails(context, errorMessage);

            return TypedResults.BadRequest(problemDetails);
        }
    }

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> is successful, it is returned.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> throws an unhandled exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    ///         </description>
    ///     </item>
    /// </list>
    /// </summary>
    public static Results<TIResult1, TIResult2, TIResult3, TIResult4, BadRequest<ProblemDetails>> Run<TIResult1, TIResult2, TIResult3, TIResult4>(
        Func<Results<TIResult1, TIResult2, TIResult3, TIResult4>> func, HttpContext context, Func<string> errorMessage)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TIResult3 : IResult
        where TIResult4 : IResult
    {
        try
        {
            var results = func();
            return results.Result switch
            {
                TIResult1 result1 => result1,
                TIResult2 result2 => result2,
                TIResult3 result3 => result3,
                TIResult4 result4 => result4,
                _                 => throw new InvalidOperationException("Unexpected result type.")
            };
        }
        catch (Exception exception)
        {
            var problemDetails = exception.ToProblemDetails(context, errorMessage());

            return TypedResults.BadRequest(problemDetails);
        }
    }

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> is successful, it is returned.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> throws an unhandled exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    ///         </description>
    ///     </item>
    /// </list>
    /// </summary>
    public static Results<TIResult1, TIResult2, TIResult3, TIResult4, TIResult5, BadRequest<ProblemDetails>> Run<TIResult1, TIResult2, TIResult3, TIResult4, TIResult5>(
        Func<Results<TIResult1, TIResult2, TIResult3, TIResult4, TIResult5>> func, HttpContext context, string errorMessage)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TIResult3 : IResult
        where TIResult4 : IResult
        where TIResult5 : IResult
    {
        try
        {
            var results = func();
            return results.Result switch
            {
                TIResult1 result1 => result1,
                TIResult2 result2 => result2,
                TIResult3 result3 => result3,
                TIResult4 result4 => result4,
                TIResult5 result5 => result5,
                _                 => throw new InvalidOperationException("Unexpected result type.")
            };
        }
        catch (Exception exception)
        {
            var problemDetails = exception.ToProblemDetails(context, errorMessage);

            return TypedResults.BadRequest(problemDetails);
        }
    }

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> is successful, it is returned.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> throws an unhandled exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    ///         </description>
    ///     </item>
    /// </list>
    /// </summary>
    public static Results<TIResult1, TIResult2, TIResult3, TIResult4, TIResult5, BadRequest<ProblemDetails>> Run<TIResult1, TIResult2, TIResult3, TIResult4, TIResult5>(
        Func<Results<TIResult1, TIResult2, TIResult3, TIResult4, TIResult5>> func, HttpContext context, Func<string> errorMessage)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TIResult3 : IResult
        where TIResult4 : IResult
        where TIResult5 : IResult
    {
        try
        {
            var results = func();
            return results.Result switch
            {
                TIResult1 result1 => result1,
                TIResult2 result2 => result2,
                TIResult3 result3 => result3,
                TIResult4 result4 => result4,
                TIResult5 result5 => result5,
                _                 => throw new InvalidOperationException("Unexpected result type.")
            };
        }
        catch (Exception exception)
        {
            var problemDetails = exception.ToProblemDetails(context, errorMessage());

            return TypedResults.BadRequest(problemDetails);
        }
    }

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> is successful, it is returned.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> throws an unhandled exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    ///         </description>
    ///     </item>
    /// </list>
    /// </summary>
    public static async Task<Results<TIResult, BadRequest<ProblemDetails>>> RunAsync<TIResult>(
        Func<Task<TIResult>> func, HttpContext context, string errorMessage)
        where TIResult : IResult
    {
        try
        {
            var value = await func();
            return value;
        }
        catch (Exception exception)
        {
            var problemDetails = exception.ToProblemDetails(context, errorMessage);

            return TypedResults.BadRequest(problemDetails);
        }
    }

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> is successful, it is returned.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> throws an unhandled exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    ///         </description>
    ///     </item>
    /// </list>
    /// </summary>
    public static async Task<Results<TIResult, BadRequest<ProblemDetails>>> RunAsync<TIResult>(
        Func<Task<TIResult>> func, HttpContext context, Func<string> errorMessage)
        where TIResult : IResult
    {
        try
        {
            var result = await func();
            return result;
        }
        catch (Exception exception)
        {
            var problemDetails = exception.ToProblemDetails(context, errorMessage());

            return TypedResults.BadRequest(problemDetails);
        }
    }

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> is successful, it is returned.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> throws an unhandled exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    ///         </description>
    ///     </item>
    /// </list>
    /// </summary>
    public static async Task<Results<TIResult1, TIResult2, BadRequest<ProblemDetails>>> RunAsync<TIResult1, TIResult2>(
        Func<Task<Results<TIResult1, TIResult2>>> func, HttpContext context, string errorMessage)
        where TIResult1 : IResult
        where TIResult2 : IResult
    {
        try
        {
            var results = await func();
            return results.Result switch
            {
                TIResult1 result1 => result1,
                TIResult2 result2 => result2,
                _                 => throw new InvalidOperationException("Unexpected result type.")
            };
        }
        catch (Exception exception)
        {
            var problemDetails = exception.ToProblemDetails(context, errorMessage);

            return TypedResults.BadRequest(problemDetails);
        }
    }

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> is successful, it is returned.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> throws an unhandled exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    ///         </description>
    ///     </item>
    /// </list>
    /// </summary>
    public static async Task<Results<TIResult1, TIResult2, BadRequest<ProblemDetails>>> RunAsync<TIResult1, TIResult2>(
        Func<Task<Results<TIResult1, TIResult2>>> func, HttpContext context, Func<string> errorMessage)
        where TIResult1 : IResult
        where TIResult2 : IResult
    {
        try
        {
            var results = await func();
            return results.Result switch
            {
                TIResult1 result1 => result1,
                TIResult2 result2 => result2,
                _                 => throw new InvalidOperationException("Unexpected result type.")
            };
        }
        catch (Exception exception)
        {
            var problemDetails = exception.ToProblemDetails(context, errorMessage());

            return TypedResults.BadRequest(problemDetails);
        }
    }

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> is successful, it is returned.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> throws an unhandled exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    ///         </description>
    ///     </item>
    /// </list>
    /// </summary>
    public static async Task<Results<TIResult1, TIResult2, TIResult3, BadRequest<ProblemDetails>>> RunAsync<TIResult1, TIResult2, TIResult3>(
        Func<Task<Results<TIResult1, TIResult2, TIResult3>>> func, HttpContext context, string errorMessage)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TIResult3 : IResult
    {
        try
        {
            var results = await func();
            return results.Result switch
            {
                TIResult1 result1 => result1,
                TIResult2 result2 => result2,
                TIResult3 result3 => result3,
                _                 => throw new InvalidOperationException("Unexpected result type.")
            };
        }
        catch (Exception exception)
        {
            var problemDetails = exception.ToProblemDetails(context, errorMessage);

            return TypedResults.BadRequest(problemDetails);
        }
    }

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> is successful, it is returned.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> throws an unhandled exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    ///         </description>
    ///     </item>
    /// </list>
    /// </summary>
    public static async Task<Results<TIResult1, TIResult2, TIResult3, BadRequest<ProblemDetails>>> RunAsync<TIResult1, TIResult2, TIResult3>(
        Func<Task<Results<TIResult1, TIResult2, TIResult3>>> func, HttpContext context, Func<string> errorMessage)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TIResult3 : IResult
    {
        try
        {
            var results = await func();
            return results.Result switch
            {
                TIResult1 result1 => result1,
                TIResult2 result2 => result2,
                TIResult3 result3 => result3,
                _                 => throw new InvalidOperationException("Unexpected result type.")
            };
        }
        catch (Exception exception)
        {
            var problemDetails = exception.ToProblemDetails(context, errorMessage());

            return TypedResults.BadRequest(problemDetails);
        }
    }

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> is successful, it is returned.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> throws an unhandled exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    ///         </description>
    ///     </item>
    /// </list>
    /// </summary>
    public static async Task<Results<TIResult1, TIResult2, TIResult3, TIResult4, BadRequest<ProblemDetails>>> RunAsync<TIResult1, TIResult2, TIResult3, TIResult4>(
        Func<Task<Results<TIResult1, TIResult2, TIResult3, TIResult4>>> func, HttpContext context, string errorMessage)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TIResult3 : IResult
        where TIResult4 : IResult
    {
        try
        {
            var results = await func();
            return results.Result switch
            {
                TIResult1 result1 => result1,
                TIResult2 result2 => result2,
                TIResult3 result3 => result3,
                TIResult4 result4 => result4,
                _                 => throw new InvalidOperationException("Unexpected result type.")
            };
        }
        catch (Exception exception)
        {
            var problemDetails = exception.ToProblemDetails(context, errorMessage);

            return TypedResults.BadRequest(problemDetails);
        }
    }

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> is successful, it is returned.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> throws an unhandled exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    ///         </description>
    ///     </item>
    /// </list>
    /// </summary>
    public static async Task<Results<TIResult1, TIResult2, TIResult3, TIResult4, BadRequest<ProblemDetails>>> RunAsync<TIResult1, TIResult2, TIResult3, TIResult4>(
        Func<Task<Results<TIResult1, TIResult2, TIResult3, TIResult4>>> func, HttpContext context, Func<string> errorMessage)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TIResult3 : IResult
        where TIResult4 : IResult
    {
        try
        {
            var results = await func();
            return results.Result switch
            {
                TIResult1 result1 => result1,
                TIResult2 result2 => result2,
                TIResult3 result3 => result3,
                TIResult4 result4 => result4,
                _                 => throw new InvalidOperationException("Unexpected result type.")
            };
        }
        catch (Exception exception)
        {
            var problemDetails = exception.ToProblemDetails(context, errorMessage());

            return TypedResults.BadRequest(problemDetails);
        }
    }

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> is successful, it is returned.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> throws an unhandled exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    ///         </description>
    ///     </item>
    /// </list>
    /// </summary>
    public static async Task<Results<TIResult1, TIResult2, TIResult3, TIResult4, TIResult5, BadRequest<ProblemDetails>>> RunAsync<TIResult1, TIResult2, TIResult3, TIResult4, TIResult5>(
        Func<Task<Results<TIResult1, TIResult2, TIResult3, TIResult4, TIResult5>>> func, HttpContext context, string errorMessage)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TIResult3 : IResult
        where TIResult4 : IResult
        where TIResult5 : IResult
    {
        try
        {
            var results = await func();
            return results.Result switch
            {
                TIResult1 result1 => result1,
                TIResult2 result2 => result2,
                TIResult3 result3 => result3,
                TIResult4 result4 => result4,
                TIResult5 result5 => result5,
                _                 => throw new InvalidOperationException("Unexpected result type.")
            };
        }
        catch (Exception exception)
        {
            var problemDetails = exception.ToProblemDetails(context, errorMessage);

            return TypedResults.BadRequest(problemDetails);
        }
    }

    /// <summary>
    /// Execute the <paramref name="func"/>.
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> is successful, it is returned.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         If the <paramref name="func"/> throws an unhandled exception, a <see cref="BadRequest{ProblemDetails}"/> is returned.
    ///         </description>
    ///     </item>
    /// </list>
    /// </summary>
    public static async Task<Results<TIResult1, TIResult2, TIResult3, TIResult4, TIResult5, BadRequest<ProblemDetails>>> RunAsync<TIResult1, TIResult2, TIResult3, TIResult4, TIResult5>(
        Func<Task<Results<TIResult1, TIResult2, TIResult3, TIResult4, TIResult5>>> func, HttpContext context, Func<string> errorMessage)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TIResult3 : IResult
        where TIResult4 : IResult
        where TIResult5 : IResult
    {
        try
        {
            var results = await func();
            return results.Result switch
            {
                TIResult1 result1 => result1,
                TIResult2 result2 => result2,
                TIResult3 result3 => result3,
                TIResult4 result4 => result4,
                TIResult5 result5 => result5,
                _                 => throw new InvalidOperationException("Unexpected result type.")
            };
        }
        catch (Exception exception)
        {
            var problemDetails = exception.ToProblemDetails(context, errorMessage());

            return TypedResults.BadRequest(problemDetails);
        }
    }
}
