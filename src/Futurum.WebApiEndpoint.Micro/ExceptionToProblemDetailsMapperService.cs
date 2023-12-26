using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Futurum.WebApiEndpoint.Micro;

/// <summary>
/// Maps exceptions to <see cref="ProblemDetails"/> based on registered mappers.
/// </summary>
public interface IExceptionToProblemDetailsMapperService
{
    /// <summary>
    /// Adds a mapper function for handling a specific type of exception.
    /// <para></para>
    /// This will override any previously registered mapper for the same exception type.
    /// </summary>
    /// <typeparam name="TException">The type of exception to handle.</typeparam>
    /// <param name="mapper">The mapper function that converts the exception to a ProblemDetails.</param>
    void Add<TException>(Func<TException, HttpContext, string?, ProblemDetails> mapper)
        where TException : Exception;

    /// <summary>
    /// Overrides the default exception mapper.
    /// </summary>
    /// <param name="mapper">The mapper function that converts the exception to a ProblemDetails.</param>
    void OverrideDefault(Func<Exception, HttpContext, string?, ProblemDetails> mapper);

    ProblemDetails Map(Exception exception, HttpContext context, string? errorMessage = null);
}

/// <inheritdoc/>
public class ExceptionToProblemDetailsMapperService : IExceptionToProblemDetailsMapperService
{
    private readonly Dictionary<Type, Func<Exception, HttpContext, string?, ProblemDetails>> _mappers;

    private Func<Exception, HttpContext, string?, ProblemDetails> _default;

    public ExceptionToProblemDetailsMapperService()
    {
        _mappers = new Dictionary<Type, Func<Exception, HttpContext, string?, ProblemDetails>>
        {
            { typeof(KeyNotFoundException), KeyNotFoundException }
        };

        _default = DefaultException;
    }

    /// <inheritdoc/>
    public void Add<TException>(Func<TException, HttpContext, string?, ProblemDetails> mapper)
        where TException : Exception
    {
        _mappers.Remove(typeof(TException));
        _mappers.Add(typeof(TException), (exception, context, errorMessage) => mapper((TException)exception, context, errorMessage));
    }

    /// <inheritdoc/>
    public void OverrideDefault(Func<Exception, HttpContext, string?, ProblemDetails> mapper)
    {
        _default = mapper;
    }

    public ProblemDetails Map(Exception exception, HttpContext context, string? errorMessage = null)
    {
        if (_mappers.TryGetValue(exception.GetType(), out var mapper))
        {
            return mapper(exception, context, errorMessage);
        }

        return _default(exception, context, errorMessage);
    }

    private ProblemDetails KeyNotFoundException(Exception exception, HttpContext context, string? errorMessage = null)
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
            Status = StatusCodes.Status400BadRequest,
            Title = ReasonPhrases.GetReasonPhrase(StatusCodes.Status400BadRequest)
        };
}
