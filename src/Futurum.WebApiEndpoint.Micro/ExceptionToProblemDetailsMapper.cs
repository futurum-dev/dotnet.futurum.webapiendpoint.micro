using System.Net;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Futurum.WebApiEndpoint.Micro;

/// <summary>
/// Maps exceptions to <see cref="ProblemDetails"/> based on registered mappers.
/// </summary>
public static class ExceptionToProblemDetailsMapper
{
    private static Func<Exception, HttpContext, string?, ProblemDetails> _default = DefaultException;

    private static readonly Dictionary<Type, Func<Exception, HttpContext, string?, ProblemDetails>> Mappers = new()
    {
        { typeof(KeyNotFoundException), KeyNotFoundException }
    };

    /// <summary>
    /// Adds a mapper function for handling a specific type of exception.
    /// <para></para>
    /// This will override any previously registered mapper for the same type.
    /// </summary>
    /// <typeparam name="TException">The type of exception to handle.</typeparam>
    /// <param name="mapper">The mapper function that converts the exception to a ProblemDetails.</param>
    public static void Add<TException>(Func<TException, HttpContext, string?, ProblemDetails> mapper)
        where TException : Exception
    {
        Mappers.Remove(typeof(TException));
        Mappers.Add(typeof(TException), (exception, context, errorMessage) => mapper((TException)exception, context, errorMessage));
    }

    /// <summary>
    /// Overrides the default exception mapper.
    /// </summary>
    /// <param name="mapper">The mapper function that converts the exception to a ProblemDetails.</param>
    public static void OverrideDefault(Func<Exception, HttpContext, string?, ProblemDetails> mapper)
    {
        _default = mapper;
    }

    public static ProblemDetails ToProblemDetails(this Exception exception, HttpContext context, string? errorMessage = null)
    {
        if (Mappers.TryGetValue(exception.GetType(), out var mapper))
        {
            return mapper(exception, context, errorMessage);
        }

        return _default(exception, context, errorMessage);
    }

    private static ProblemDetails KeyNotFoundException(Exception exception, HttpContext context, string? errorMessage = null)
    {
        if (exception is not KeyNotFoundException keyNotFoundException)
        {
            return _default(exception, context, errorMessage);
        }

        return new ProblemDetails
        {
            Detail = !string.IsNullOrEmpty(errorMessage) ? $"{errorMessage};{keyNotFoundException.Message}" : keyNotFoundException.Message,
            Instance = context.Request.Path,
            Status = StatusCodes.Status404NotFound,
            Title = "Key Not Found",
        };
    }

    public static ProblemDetails DefaultException(Exception exception, HttpContext context, string? errorMessage) =>
        new()
        {
            Detail = !string.IsNullOrEmpty(errorMessage) ? $"{errorMessage};{exception.Message}" : exception.Message,
            Instance = context.Request.Path,
            Status = (int)HttpStatusCode.BadRequest,
            Title = ReasonPhrases.GetReasonPhrase((int)HttpStatusCode.BadRequest)
        };
}
