using Futurum.Core.Result;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.Micro;

public static partial class WebApiResultsExtensions
{
    public static Results<TIResult1, TIResult2, TIResult3, TIResult4, TIResult5, BadRequest<ProblemDetails>> ToWebApi<TIResult1, TIResult2, TIResult3, TIResult4, TIResult5>(
        this Result<Results<TIResult1, TIResult2, TIResult3, TIResult4, TIResult5>> result,
        HttpContext context)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TIResult3 : IResult
        where TIResult4 : IResult
        where TIResult5 : IResult =>
        result.ToWebApi(context, ToWebApiBadRequest);

    public static Results<TIResult1, TIResult2, TIResult3, TIResult4, TIResult5, TErrorIResult> ToWebApi<TIResult1, TIResult2, TIResult3, TIResult4, TIResult5, TErrorIResult>(
        this Result<Results<TIResult1, TIResult2, TIResult3, TIResult4, TIResult5>> result,
        HttpContext context,
        Func<IResultError, HttpContext, TErrorIResult> resultErrorHandler)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TIResult3 : IResult
        where TIResult4 : IResult
        where TIResult5 : IResult
        where TErrorIResult : IResult =>
        result.Switch(value =>
                      {
                          return value.Result switch
                          {
                              TIResult1 valueResult1 => valueResult1,
                              TIResult2 valueResult2 => valueResult2,
                              TIResult3 valueResult3 => valueResult3,
                              TIResult4 valueResult4 => valueResult4,
                              TIResult5 valueResult5 => valueResult5,
                              _                      => throw new InvalidOperationException()
                          };
                      },
                      error => (Results<TIResult1, TIResult2, TIResult3, TIResult4, TIResult5, TErrorIResult>)resultErrorHandler(error, context));

    public static Task<Results<TIResult1, TIResult2, TIResult3, TIResult4, TIResult5, BadRequest<ProblemDetails>>> ToWebApiAsync<TIResult1, TIResult2, TIResult3, TIResult4, TIResult5>(
        this Task<Result<Results<TIResult1, TIResult2, TIResult3, TIResult4, TIResult5>>> resultTask,
        HttpContext context)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TIResult3 : IResult
        where TIResult4 : IResult
        where TIResult5 : IResult =>
        resultTask.ToWebApiAsync(context, ToWebApiBadRequest);

    public static Task<Results<TIResult1, TIResult2, TIResult3, TIResult4, TIResult5, TErrorIResult>> ToWebApiAsync<TIResult1, TIResult2, TIResult3, TIResult4, TIResult5, TErrorIResult>(
        this Task<Result<Results<TIResult1, TIResult2, TIResult3, TIResult4, TIResult5>>> resultTask,
        HttpContext context,
        Func<IResultError, HttpContext, TErrorIResult> resultErrorHandler)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TIResult3 : IResult
        where TIResult4 : IResult
        where TIResult5 : IResult
        where TErrorIResult : IResult =>
        resultTask.SwitchAsync(value =>
                               {
                                   return value.Result switch
                                   {
                                       TIResult1 valueResult1 => valueResult1,
                                       TIResult2 valueResult2 => valueResult2,
                                       TIResult3 valueResult3 => valueResult3,
                                       TIResult4 valueResult4 => valueResult4,
                                       TIResult5 valueResult5 => valueResult5,
                                       _                      => throw new InvalidOperationException()
                                   };
                               },
                               error => (Results<TIResult1, TIResult2, TIResult3, TIResult4, TIResult5, TErrorIResult>)resultErrorHandler(error, context));
}