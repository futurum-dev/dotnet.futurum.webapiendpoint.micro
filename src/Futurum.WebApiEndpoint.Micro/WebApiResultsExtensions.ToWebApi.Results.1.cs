using Futurum.Core.Option;
using Futurum.Core.Result;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.Micro;

public static partial class WebApiResultsExtensions
{
    public static Results<TIResult, BadRequest<ProblemDetails>> ToWebApi<TIResult>(this Result<TIResult> result,
                                                                                   HttpContext context)
        where TIResult : IResult =>
        result.ToWebApi(context, ToWebApiBadRequest);

    public static Results<TIResult, TErrorIResult> ToWebApi<TIResult, TErrorIResult>(this Result<TIResult> result,
                                                                                     HttpContext context,
                                                                                     Func<IResultError, HttpContext, TErrorIResult> resultErrorHandler)
        where TIResult : IResult
        where TErrorIResult : IResult =>
        result.Switch(value => (Results<TIResult, TErrorIResult>)value,
                      error => (Results<TIResult, TErrorIResult>)resultErrorHandler(error, context));

    public static Results<TIResult, TErrorIResult, BadRequest<ProblemDetails>> ToWebApi<TIResult, TErrorIResult>(
        this Result<TIResult> result, HttpContext context,
        Func<IResultError, HttpContext, Option<TErrorIResult>> resultErrorHandler)
        where TIResult : IResult
        where TErrorIResult : IResult =>
        result.ToWebApi(context, (_, x) => x, resultErrorHandler);

    public static Results<TIResult, TErrorIResult1, TErrorIResult2, BadRequest<ProblemDetails>> ToWebApi<TIResult, TErrorIResult1, TErrorIResult2>(
        this Result<TIResult> result, HttpContext context,
        Func<IResultError, HttpContext, Option<TErrorIResult1>> resultErrorHandler1,
        Func<IResultError, HttpContext, Option<TErrorIResult2>> resultErrorHandler2)
        where TIResult : IResult
        where TErrorIResult1 : IResult
        where TErrorIResult2 : IResult =>
        result.ToWebApi(context, (_, x) => x, resultErrorHandler1, resultErrorHandler2);

    public static Results<TIResult, TErrorIResult1, TErrorIResult2, TErrorIResult3, BadRequest<ProblemDetails>> ToWebApi<TIResult, TErrorIResult1, TErrorIResult2, TErrorIResult3>(
        this Result<TIResult> result, HttpContext context,
        Func<IResultError, HttpContext, Option<TErrorIResult1>> resultErrorHandler1,
        Func<IResultError, HttpContext, Option<TErrorIResult2>> resultErrorHandler2,
        Func<IResultError, HttpContext, Option<TErrorIResult3>> resultErrorHandler3)
        where TIResult : IResult
        where TErrorIResult1 : IResult
        where TErrorIResult2 : IResult
        where TErrorIResult3 : IResult =>
        result.ToWebApi(context, (_, x) => x, resultErrorHandler1, resultErrorHandler2, resultErrorHandler3);

    public static Results<TIResult, TErrorIResult1, TErrorIResult2, TErrorIResult3, TErrorIResult4, BadRequest<ProblemDetails>> ToWebApi<
        TIResult, TErrorIResult1, TErrorIResult2, TErrorIResult3, TErrorIResult4>(
        this Result<TIResult> result, HttpContext context,
        Func<IResultError, HttpContext, Option<TErrorIResult1>> resultErrorHandler1,
        Func<IResultError, HttpContext, Option<TErrorIResult2>> resultErrorHandler2,
        Func<IResultError, HttpContext, Option<TErrorIResult3>> resultErrorHandler3,
        Func<IResultError, HttpContext, Option<TErrorIResult4>> resultErrorHandler4)
        where TIResult : IResult
        where TErrorIResult1 : IResult
        where TErrorIResult2 : IResult
        where TErrorIResult3 : IResult
        where TErrorIResult4 : IResult =>
        result.ToWebApi(context, (_, x) => x, resultErrorHandler1, resultErrorHandler2, resultErrorHandler3, resultErrorHandler4);

    public static Task<Results<TIResult, BadRequest<ProblemDetails>>> ToWebApiAsync<TIResult>(this Task<Result<TIResult>> resultTask,
                                                                                              HttpContext context)
        where TIResult : IResult =>
        resultTask.ToWebApiAsync(context, ToWebApiBadRequest);

    public static Task<Results<TIResult, TErrorIResult>> ToWebApiAsync<TIResult, TErrorIResult>(this Task<Result<TIResult>> resultTask,
                                                                                                HttpContext context,
                                                                                                Func<IResultError, HttpContext, TErrorIResult> resultErrorHandler)
        where TIResult : IResult
        where TErrorIResult : IResult =>
        resultTask.SwitchAsync(value => (Results<TIResult, TErrorIResult>)value,
                               error => (Results<TIResult, TErrorIResult>)resultErrorHandler(error, context));

    public static Task<Results<TIResult, TErrorIResult, BadRequest<ProblemDetails>>> ToWebApiAsync<TIResult, TErrorIResult>(
        this Task<Result<TIResult>> result,
        HttpContext context,
        Func<IResultError, HttpContext, Option<TErrorIResult>> resultErrorHandler)
        where TIResult : IResult
        where TErrorIResult : IResult =>
        result.ToWebApiAsync(context, (_, x) => x, resultErrorHandler);

    public static Task<Results<TIResult, TErrorIResult1, TErrorIResult2, BadRequest<ProblemDetails>>> ToWebApiAsync<TIResult, TErrorIResult1, TErrorIResult2>(
        this Task<Result<TIResult>> result, HttpContext context,
        Func<IResultError, HttpContext, Option<TErrorIResult1>> resultErrorHandler1,
        Func<IResultError, HttpContext, Option<TErrorIResult2>> resultErrorHandler2)
        where TIResult : IResult
        where TErrorIResult1 : IResult
        where TErrorIResult2 : IResult =>
        result.ToWebApiAsync(context, (_, x) => x, resultErrorHandler1, resultErrorHandler2);

    public static Task<Results<TIResult, TErrorIResult1, TErrorIResult2, TErrorIResult3, BadRequest<ProblemDetails>>> ToWebApiAsync<TIResult, TErrorIResult1, TErrorIResult2, TErrorIResult3>(
        this Task<Result<TIResult>> result, HttpContext context,
        Func<IResultError, HttpContext, Option<TErrorIResult1>> resultErrorHandler1,
        Func<IResultError, HttpContext, Option<TErrorIResult2>> resultErrorHandler2,
        Func<IResultError, HttpContext, Option<TErrorIResult3>> resultErrorHandler3)
        where TIResult : IResult
        where TErrorIResult1 : IResult
        where TErrorIResult2 : IResult
        where TErrorIResult3 : IResult =>
        result.ToWebApiAsync(context, (_, x) => x, resultErrorHandler1, resultErrorHandler2, resultErrorHandler3);

    public static Task<Results<TIResult, TErrorIResult1, TErrorIResult2, TErrorIResult3, TErrorIResult4, BadRequest<ProblemDetails>>> ToWebApiAsync<
        TIResult, TErrorIResult1, TErrorIResult2, TErrorIResult3, TErrorIResult4>(
        this Task<Result<TIResult>> result, HttpContext context,
        Func<IResultError, HttpContext, Option<TErrorIResult1>> resultErrorHandler1,
        Func<IResultError, HttpContext, Option<TErrorIResult2>> resultErrorHandler2,
        Func<IResultError, HttpContext, Option<TErrorIResult3>> resultErrorHandler3,
        Func<IResultError, HttpContext, Option<TErrorIResult4>> resultErrorHandler4)
        where TIResult : IResult
        where TErrorIResult1 : IResult
        where TErrorIResult2 : IResult
        where TErrorIResult3 : IResult
        where TErrorIResult4 : IResult =>
        result.ToWebApiAsync(context, (_, x) => x, resultErrorHandler1, resultErrorHandler2, resultErrorHandler3, resultErrorHandler4);
}