using Futurum.Core.Option;
using Futurum.Core.Result;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.Micro;

public static partial class WebApiResultsExtensions
{
    public static Results<TIResult, TErrorIResult, BadRequest<ProblemDetails>> ToWebApi<TIResult, TErrorIResult>(
        this Result result, HttpContext context,
        Func<HttpContext, TIResult> resultHandler,
        Func<IResultError, HttpContext, Option<TErrorIResult>> resultErrorHandler)
        where TIResult : IResult
        where TErrorIResult : IResult =>
        result.Switch(() => (Results<TIResult, TErrorIResult, BadRequest<ProblemDetails>>)resultHandler(context),
                      error => resultErrorHandler(error, context)
                               .Map(x => (Results<TIResult, TErrorIResult, BadRequest<ProblemDetails>>)x)
                               .GetValueOrDefault(() => error.ToWebApiBadRequest(context)));

    public static Results<TIResult, TErrorIResult, BadRequest<ProblemDetails>> ToWebApi<T, TIResult, TErrorIResult>(
        this Result<T> result, HttpContext context,
        Func<HttpContext, T, TIResult> resultHandler,
        Func<IResultError, HttpContext, Option<TErrorIResult>> resultErrorHandler)
        where TIResult : IResult
        where TErrorIResult : IResult =>
        result.Switch(value => (Results<TIResult, TErrorIResult, BadRequest<ProblemDetails>>)resultHandler(context, value),
                      error => resultErrorHandler(error, context)
                               .Map(x => (Results<TIResult, TErrorIResult, BadRequest<ProblemDetails>>)x)
                               .GetValueOrDefault(() => error.ToWebApiBadRequest(context)));

    public static Task<Results<TIResult, TErrorIResult, BadRequest<ProblemDetails>>> ToWebApiAsync<TIResult, TErrorIResult>(
        this Task<Result> result, HttpContext context,
        Func<HttpContext, TIResult> resultHandler,
        Func<IResultError, HttpContext, Option<TErrorIResult>> resultErrorHandler)
        where TIResult : IResult
        where TErrorIResult : IResult =>
        result.SwitchAsync(() => (Results<TIResult, TErrorIResult, BadRequest<ProblemDetails>>)resultHandler(context),
                           error => resultErrorHandler(error, context)
                                    .Map(x => (Results<TIResult, TErrorIResult, BadRequest<ProblemDetails>>)x)
                                    .GetValueOrDefault(() => error.ToWebApiBadRequest(context)));

    public static Task<Results<TIResult, TErrorIResult, BadRequest<ProblemDetails>>> ToWebApiAsync<T, TIResult, TErrorIResult>(
        this Task<Result<T>> result, HttpContext context,
        Func<HttpContext, T, TIResult> resultHandler,
        Func<IResultError, HttpContext, Option<TErrorIResult>> resultErrorHandler)
        where TIResult : IResult
        where TErrorIResult : IResult =>
        result.SwitchAsync(value => (Results<TIResult, TErrorIResult, BadRequest<ProblemDetails>>)resultHandler(context, value),
                           error => resultErrorHandler(error, context)
                                    .Map(x => (Results<TIResult, TErrorIResult, BadRequest<ProblemDetails>>)x)
                                    .GetValueOrDefault(() => error.ToWebApiBadRequest(context)));
}