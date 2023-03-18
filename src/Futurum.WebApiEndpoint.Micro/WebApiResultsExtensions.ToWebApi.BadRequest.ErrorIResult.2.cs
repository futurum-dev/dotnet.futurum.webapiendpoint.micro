using Futurum.Core.Option;
using Futurum.Core.Result;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.Micro;

public static partial class WebApiResultsExtensions
{
    public static Results<TIResult, TErrorIResult1, TErrorIResult2, BadRequest<ProblemDetails>> ToWebApi<TIResult, TErrorIResult1, TErrorIResult2>(
        this Result result, HttpContext context,
        Func<HttpContext, TIResult> resultHandler,
        Func<IResultError, HttpContext, Option<TErrorIResult1>> resultErrorHandler1,
        Func<IResultError, HttpContext, Option<TErrorIResult2>> resultErrorHandler2)
        where TIResult : IResult
        where TErrorIResult1 : IResult
        where TErrorIResult2 : IResult =>
        result.Switch(() => (Results<TIResult, TErrorIResult1, TErrorIResult2, BadRequest<ProblemDetails>>)resultHandler(context),
                      error => resultErrorHandler1(error, context).Map(x => (Results<TIResult, TErrorIResult1, TErrorIResult2, BadRequest<ProblemDetails>>)x)
                                                                  .OrElse(() => resultErrorHandler2(error, context)
                                                                              .Map(x => (Results<TIResult, TErrorIResult1, TErrorIResult2, BadRequest<ProblemDetails>>)x))
                                                                  .GetValueOrDefault(() => error.ToWebApiBadRequest(context)));

    public static Results<TIResult, TErrorIResult1, TErrorIResult2, BadRequest<ProblemDetails>> ToWebApi<T, TIResult, TErrorIResult1, TErrorIResult2>(
        this Result<T> result, HttpContext context,
        Func<HttpContext, T, TIResult> resultHandler,
        Func<IResultError, HttpContext, Option<TErrorIResult1>> resultErrorHandler1,
        Func<IResultError, HttpContext, Option<TErrorIResult2>> resultErrorHandler2)
        where TIResult : IResult
        where TErrorIResult1 : IResult
        where TErrorIResult2 : IResult =>
        result.Switch(value => (Results<TIResult, TErrorIResult1, TErrorIResult2, BadRequest<ProblemDetails>>)resultHandler(context, value),
                      error => resultErrorHandler1(error, context).Map(x => (Results<TIResult, TErrorIResult1, TErrorIResult2, BadRequest<ProblemDetails>>)x)
                                                                  .OrElse(() => resultErrorHandler2(error, context)
                                                                              .Map(x => (Results<TIResult, TErrorIResult1, TErrorIResult2, BadRequest<ProblemDetails>>)x))
                                                                  .GetValueOrDefault(() => error.ToWebApiBadRequest(context)));

    public static Task<Results<TIResult, TErrorIResult1, TErrorIResult2, BadRequest<ProblemDetails>>> ToWebApiAsync<TIResult, TErrorIResult1, TErrorIResult2>(
        this Task<Result> result, HttpContext context,
        Func<HttpContext, TIResult> resultHandler,
        Func<IResultError, HttpContext, Option<TErrorIResult1>> resultErrorHandler1,
        Func<IResultError, HttpContext, Option<TErrorIResult2>> resultErrorHandler2)
        where TIResult : IResult
        where TErrorIResult1 : IResult
        where TErrorIResult2 : IResult =>
        result.SwitchAsync(() => (Results<TIResult, TErrorIResult1, TErrorIResult2, BadRequest<ProblemDetails>>)resultHandler(context),
                           error => resultErrorHandler1(error, context)
                                    .Map(x => (Results<TIResult, TErrorIResult1, TErrorIResult2, BadRequest<ProblemDetails>>)x)
                                    .OrElse(() => resultErrorHandler2(error, context)
                                                .Map(x => (Results<TIResult, TErrorIResult1, TErrorIResult2, BadRequest<ProblemDetails>>)x))
                                    .GetValueOrDefault(() => error.ToWebApiBadRequest(context)));

    public static Task<Results<TIResult, TErrorIResult1, TErrorIResult2, BadRequest<ProblemDetails>>> ToWebApiAsync<T, TIResult, TErrorIResult1, TErrorIResult2>(
        this Task<Result<T>> result, HttpContext context,
        Func<HttpContext, T, TIResult> resultHandler,
        Func<IResultError, HttpContext, Option<TErrorIResult1>> resultErrorHandler1,
        Func<IResultError, HttpContext, Option<TErrorIResult2>> resultErrorHandler2)
        where TIResult : IResult
        where TErrorIResult1 : IResult
        where TErrorIResult2 : IResult =>
        result.SwitchAsync(value => (Results<TIResult, TErrorIResult1, TErrorIResult2, BadRequest<ProblemDetails>>)resultHandler(context, value),
                           error => resultErrorHandler1(error, context)
                                    .Map(x => (Results<TIResult, TErrorIResult1, TErrorIResult2, BadRequest<ProblemDetails>>)x)
                                    .OrElse(() => resultErrorHandler2(error, context)
                                                .Map(x => (Results<TIResult, TErrorIResult1, TErrorIResult2, BadRequest<ProblemDetails>>)x))
                                    .GetValueOrDefault(() => error.ToWebApiBadRequest(context)));
}