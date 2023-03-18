using System.Net;

using Futurum.Core.Result;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Futurum.WebApiEndpoint.Micro;

public static partial class WebApiResultsExtensions
{
    public static ProblemDetails ToProblemDetails(this IResultError resultError, HttpStatusCode failedStatusCode, HttpContext context) =>
        resultError switch
        {
            ResultErrorComposite resultErrorComposite => CompositeResultError(resultErrorComposite, failedStatusCode, context),
            _                                         => GeneralError(resultError, failedStatusCode, context)
        };

    private static ProblemDetails CompositeResultError(ResultErrorComposite resultErrorComposite, HttpStatusCode failedStatusCode, HttpContext context) =>
        new()
        {
            Detail = resultErrorComposite.GetErrorStringSafe(";"),
            Instance = context.Request.Path,
            Status = (int)failedStatusCode,
            Title = ReasonPhrases.GetReasonPhrase((int)failedStatusCode)
        };

    private static ProblemDetails GeneralError(IResultError resultError, HttpStatusCode failedStatusCode, HttpContext context) =>
        new()
        {
            Detail = resultError.ToErrorStringSafe(),
            Instance = context.Request.Path,
            Status = (int)failedStatusCode,
            Title = ReasonPhrases.GetReasonPhrase((int)failedStatusCode)
        };
}