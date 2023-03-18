using Futurum.Core.Result;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.Micro;

public static partial class WebApiResultsExtensions
{
    public static Results<TIResult, BadRequest<ProblemDetails>> ToWebApi<TIResult>(
        this Result result, HttpContext context,
        Func<HttpContext, TIResult> resultHandler)
        where TIResult : IResult =>
        result.Switch(() => (Results<TIResult, BadRequest<ProblemDetails>>)resultHandler(context),
                      error => error.ToWebApiBadRequest(context));

    public static Results<TIResult, BadRequest<ProblemDetails>> ToWebApi<T, TIResult>(
        this Result<T> result, HttpContext context,
        Func<HttpContext, T, TIResult> resultHandler)
        where TIResult : IResult =>
        result.Switch(value => (Results<TIResult, BadRequest<ProblemDetails>>)resultHandler(context, value),
                      error => error.ToWebApiBadRequest(context));

    public static Task<Results<TIResult, BadRequest<ProblemDetails>>> ToWebApiAsync<TIResult>(
        this Task<Result> result, HttpContext context,
        Func<HttpContext, TIResult> resultHandler)
        where TIResult : IResult =>
        result.SwitchAsync(() => (Results<TIResult, BadRequest<ProblemDetails>>)resultHandler(context),
                           error => error.ToWebApiBadRequest(context));

    public static Task<Results<TIResult, BadRequest<ProblemDetails>>> ToWebApiAsync<T, TIResult>(
        this Task<Result<T>> result, HttpContext context,
        Func<HttpContext, T, TIResult> resultHandler)
        where TIResult : IResult =>
        result.SwitchAsync(value => (Results<TIResult, BadRequest<ProblemDetails>>)resultHandler(context, value),
                           error => error.ToWebApiBadRequest(context));
}