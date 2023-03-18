using System.Net;

using Futurum.Core.Result;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.Micro;

public static partial class WebApiResultsExtensions
{
    public static BadRequest<ProblemDetails> ToWebApiBadRequest(this IResultError error, HttpContext context)
    {
        var problemDetails = error.ToProblemDetails(HttpStatusCode.BadRequest, context);
        
        return TypedResults.BadRequest(problemDetails);
    }
}