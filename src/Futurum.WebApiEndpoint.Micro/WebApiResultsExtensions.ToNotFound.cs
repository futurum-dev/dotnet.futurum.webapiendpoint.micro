using System.Net;

using Futurum.Core.Option;
using Futurum.Core.Result;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Futurum.WebApiEndpoint.Micro;

public static partial class WebApiResultsExtensions
{
    public static Option<NotFound<ProblemDetails>> ToNotFound(IResultError resultError, HttpContext context)
    {
        if (resultError is ResultErrorKeyNotFound validationResultError)
        {
            var problemDetails = new ProblemDetails
            {
                Detail = validationResultError.GetErrorStringSafe(),
                Instance = context.Request.Path,
                Status = (int)HttpStatusCode.NotFound,
                Title = ReasonPhrases.GetReasonPhrase((int)HttpStatusCode.NotFound)
            };
            
            var notFound = TypedResults.NotFound(problemDetails);

            return notFound.ToOption();
        }

        return Option.None<NotFound<ProblemDetails>>();
    }
}