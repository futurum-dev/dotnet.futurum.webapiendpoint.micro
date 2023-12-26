using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.Micro;

public interface IWebApiEndpointRunnerExceptionHandlerService
{
    BadRequest<ProblemDetails> Handle(HttpContext httpContext, Exception exception, string errorMessage);
}

public class WebApiEndpointRunnerExceptionHandlerService(ILogger<WebApiEndpointRunnerExceptionHandlerService> logger, IExceptionToProblemDetailsMapperService exceptionToProblemDetailsMapperService)
    : IWebApiEndpointRunnerExceptionHandlerService
{
    private static readonly Action<ILogger, string, string, Exception> LogErrorMessage =
        LoggerMessage.Define<string, string>(LogLevel.Error, eventId: new EventId(id: 0, name: "ERROR"), formatString: "{Path}\nException occurred:{Message}");

    public BadRequest<ProblemDetails> Handle(HttpContext httpContext, Exception exception, string errorMessage)
    {
        LogErrorMessage(logger, httpContext.Request.Path, exception.Message, exception);

        var problemDetails = exceptionToProblemDetailsMapperService.Map(exception, httpContext, errorMessage);

        return TypedResults.BadRequest(problemDetails);
    }
}
