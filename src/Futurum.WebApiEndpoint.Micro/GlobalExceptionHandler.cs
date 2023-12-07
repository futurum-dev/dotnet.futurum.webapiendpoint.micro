using Microsoft.AspNetCore.Diagnostics;

namespace Futurum.WebApiEndpoint.Micro;

/// <summary>
/// Global exception handler, for unhandled exceptions
/// <remarks>Utilises <see cref="IExceptionToProblemDetailsMapperService"/></remarks>
/// </summary>
public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IExceptionToProblemDetailsMapperService exceptionToProblemDetailsMapperService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        var problemDetails = exceptionToProblemDetailsMapperService.Map(exception, httpContext, null);

        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
