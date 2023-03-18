using Futurum.Core.Option;
using Futurum.Core.Result;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.Micro;

public static partial class WebApiResultsExtensions
{
    public static Results<TIResult1, TIResult2, TIResult3, TIResult4, BadRequest<ProblemDetails>> ToWebApi<TIResult1, TIResult2, TIResult3, TIResult4>(
        this Result<Results<TIResult1, TIResult2, TIResult3, TIResult4>> result,
        HttpContext context)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TIResult3 : IResult
        where TIResult4 : IResult =>
        result.ToWebApi(context, ToWebApiBadRequest);

    public static Results<TIResult1, TIResult2, TIResult3, TIResult4, TErrorIResult> ToWebApi<TIResult1, TIResult2, TIResult3, TIResult4, TErrorIResult>(
        this Result<Results<TIResult1, TIResult2, TIResult3, TIResult4>> result,
        HttpContext context,
        Func<IResultError, HttpContext, TErrorIResult> resultErrorHandler)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TIResult3 : IResult
        where TIResult4 : IResult
        where TErrorIResult : IResult =>
        result.Switch(value =>
                      {
                          return value.Result switch
                          {
                              TIResult1 valueResult1 => valueResult1,
                              TIResult2 valueResult2 => valueResult2,
                              TIResult3 valueResult3 => valueResult3,
                              TIResult4 valueResult4 => valueResult4,
                              _                      => throw new InvalidOperationException()
                          };
                      },
                      error => (Results<TIResult1, TIResult2, TIResult3, TIResult4, TErrorIResult>)resultErrorHandler(error, context));

    public static Results<TIResult1, TIResult2, TIResult3, TIResult4, TErrorIResult, BadRequest<ProblemDetails>> ToWebApi<TIResult1, TIResult2, TIResult3, TIResult4, TErrorIResult>(
        this Result<Results<TIResult1, TIResult2, TIResult3, TIResult4>> result,
        HttpContext context,
        Func<IResultError, HttpContext, Option<TErrorIResult>> resultErrorHandler)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TIResult3 : IResult
        where TIResult4 : IResult
        where TErrorIResult : IResult =>
        result.ToWebApi(context,
                        (error, _) => resultErrorHandler(error, context)
                                      .Map(x => (Results<TErrorIResult, BadRequest<ProblemDetails>>)x)
                                      .GetValueOrDefault(() => error.ToWebApiBadRequest(context)));

    public static Results<TIResult1, TIResult2, TIResult3, TIResult4, TErrorIResult1, TErrorIResult2> ToWebApi<TIResult1, TIResult2, TIResult3, TIResult4, TErrorIResult1, TErrorIResult2>(
        this Result<Results<TIResult1, TIResult2, TIResult3, TIResult4>> result,
        HttpContext context,
        Func<IResultError, HttpContext, Results<TErrorIResult1, TErrorIResult2>> errorFunc)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TIResult3 : IResult
        where TIResult4 : IResult
        where TErrorIResult1 : IResult
        where TErrorIResult2 : IResult =>
        result.Switch(value =>
                      {
                          return value.Result switch
                          {
                              TIResult1 valueResult1 => (Results<TIResult1, TIResult2, TIResult3, TIResult4, TErrorIResult1, TErrorIResult2>)valueResult1,
                              TIResult2 valueResult2 => (Results<TIResult1, TIResult2, TIResult3, TIResult4, TErrorIResult1, TErrorIResult2>)valueResult2,
                              TIResult3 valueResult3 => (Results<TIResult1, TIResult2, TIResult3, TIResult4, TErrorIResult1, TErrorIResult2>)valueResult3,
                              TIResult4 valueResult4 => (Results<TIResult1, TIResult2, TIResult3, TIResult4, TErrorIResult1, TErrorIResult2>)valueResult4,
                              _                      => throw new InvalidOperationException()
                          };
                      },
                      error =>
                      {
                          return errorFunc(error, context).Result switch
                          {
                              TErrorIResult1 errorResult1 => (Results<TIResult1, TIResult2, TIResult3, TIResult4, TErrorIResult1, TErrorIResult2>)errorResult1,
                              TErrorIResult2 errorResult2 => (Results<TIResult1, TIResult2, TIResult3, TIResult4, TErrorIResult1, TErrorIResult2>)errorResult2,
                              _                           => throw new InvalidOperationException()
                          };
                      });

    public static Task<Results<TIResult1, TIResult2, TIResult3, TIResult4, BadRequest<ProblemDetails>>> ToWebApiAsync<TIResult1, TIResult2, TIResult3, TIResult4>(
        this Task<Result<Results<TIResult1, TIResult2, TIResult3, TIResult4>>> resultTask,
        HttpContext context)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TIResult3 : IResult
        where TIResult4 : IResult =>
        resultTask.ToWebApiAsync(context, ToWebApiBadRequest);

    public static Task<Results<TIResult1, TIResult2, TIResult3, TIResult4, TErrorIResult>> ToWebApiAsync<TIResult1, TIResult2, TIResult3, TIResult4, TErrorIResult>(
        this Task<Result<Results<TIResult1, TIResult2, TIResult3, TIResult4>>> resultTask,
        HttpContext context,
        Func<IResultError, HttpContext, TErrorIResult> resultErrorHandler)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TIResult3 : IResult
        where TIResult4 : IResult
        where TErrorIResult : IResult =>
        resultTask.SwitchAsync(value =>
                               {
                                   return value.Result switch
                                   {
                                       TIResult1 valueResult1 => valueResult1,
                                       TIResult2 valueResult2 => valueResult2,
                                       TIResult3 valueResult3 => valueResult3,
                                       TIResult4 valueResult4 => valueResult4,
                                       _                      => throw new InvalidOperationException()
                                   };
                               },
                               error => (Results<TIResult1, TIResult2, TIResult3, TIResult4, TErrorIResult>)resultErrorHandler(error, context));

    public static Task<Results<TIResult1, TIResult2, TIResult3, TIResult4, TErrorIResult, BadRequest<ProblemDetails>>> ToWebApiAsync<TIResult1, TIResult2, TIResult3, TIResult4, TErrorIResult>(
        this Task<Result<Results<TIResult1, TIResult2, TIResult3, TIResult4>>> resultTask,
        HttpContext context,
        Func<IResultError, HttpContext, Option<TErrorIResult>> resultErrorHandler)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TIResult3 : IResult
        where TIResult4 : IResult
        where TErrorIResult : IResult =>
        resultTask.ToWebApiAsync(context,
                                 (error, _) => resultErrorHandler(error, context)
                                               .Map(x => (Results<TErrorIResult, BadRequest<ProblemDetails>>)x)
                                               .GetValueOrDefault(() => error.ToWebApiBadRequest(context)));

    public static Task<Results<TIResult1, TIResult2, TIResult3, TIResult4, TErrorIResult1, TErrorIResult2>> ToWebApiAsync<TIResult1, TIResult2, TIResult3, TIResult4, TErrorIResult1, TErrorIResult2>(
        this Task<Result<Results<TIResult1, TIResult2, TIResult3, TIResult4>>> resultTask,
        HttpContext context,
        Func<IResultError, HttpContext, Results<TErrorIResult1, TErrorIResult2>> errorFunc)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TIResult3 : IResult
        where TIResult4 : IResult
        where TErrorIResult1 : IResult
        where TErrorIResult2 : IResult =>
        resultTask.SwitchAsync(value =>
                               {
                                   return value.Result switch
                                   {
                                       TIResult1 valueResult1 => (Results<TIResult1, TIResult2, TIResult3, TIResult4, TErrorIResult1, TErrorIResult2>)valueResult1,
                                       TIResult2 valueResult2 => (Results<TIResult1, TIResult2, TIResult3, TIResult4, TErrorIResult1, TErrorIResult2>)valueResult2,
                                       TIResult3 valueResult3 => (Results<TIResult1, TIResult2, TIResult3, TIResult4, TErrorIResult1, TErrorIResult2>)valueResult3,
                                       TIResult4 valueResult4 => (Results<TIResult1, TIResult2, TIResult3, TIResult4, TErrorIResult1, TErrorIResult2>)valueResult4,
                                       _                      => throw new InvalidOperationException()
                                   };
                               },
                               error =>
                               {
                                   return errorFunc(error, context).Result switch
                                   {
                                       TErrorIResult1 errorResult1 => (Results<TIResult1, TIResult2, TIResult3, TIResult4, TErrorIResult1, TErrorIResult2>)errorResult1,
                                       TErrorIResult2 errorResult2 => (Results<TIResult1, TIResult2, TIResult3, TIResult4, TErrorIResult1, TErrorIResult2>)errorResult2,
                                       _                           => throw new InvalidOperationException()
                                   };
                               });
}