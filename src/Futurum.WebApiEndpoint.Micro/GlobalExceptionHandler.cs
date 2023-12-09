using Microsoft.AspNetCore.Diagnostics;

using static Microsoft.Extensions.Logging.LoggerMessage;

namespace Futurum.WebApiEndpoint.Micro;

/// <summary>
/// Global exception handler, for unhandled exceptions
/// <remarks>Utilises <see cref="IExceptionToProblemDetailsMapperService"/></remarks>
/// </summary>
public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IExceptionToProblemDetailsMapperService exceptionToProblemDetailsMapperService)
    : IExceptionHandler
{
    private static readonly Action<ILogger, string, Exception> LogErrorMessage = Define<string>(LogLevel.Error, eventId: new EventId(id: 0, name: "ERROR"), formatString: "Exception occurred: {Message}");

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        LogErrorMessage(logger, exception.Message, exception);

        var problemDetails = exceptionToProblemDetailsMapperService.Map(exception, httpContext, null);

        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
