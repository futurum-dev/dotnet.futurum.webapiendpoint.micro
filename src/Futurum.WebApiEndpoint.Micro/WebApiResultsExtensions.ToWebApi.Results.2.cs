using Futurum.Core.Option;
using Futurum.Core.Result;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.Micro;

public static partial class WebApiResultsExtensions
{
    public static Results<TIResult1, TIResult2, BadRequest<ProblemDetails>> ToWebApi<TIResult1, TIResult2>(this Result<Results<TIResult1, TIResult2>> result,
                                                                                                           HttpContext context)
        where TIResult1 : IResult
        where TIResult2 : IResult =>
        result.ToWebApi(context, ToWebApiBadRequest);

    public static Results<TIResult1, TIResult2, TErrorIResult> ToWebApi<TIResult1, TIResult2, TErrorIResult>(this Result<Results<TIResult1, TIResult2>> result,
                                                                                                             HttpContext context,
                                                                                                             Func<IResultError, HttpContext, TErrorIResult> resultErrorHandler)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TErrorIResult : IResult =>
        result.Switch(value =>
                      {
                          return value.Result switch
                          {
                              TIResult1 valueResult1 => valueResult1,
                              TIResult2 valueResult2 => valueResult2,
                              _                      => throw new InvalidOperationException()
                          };
                      },
                      error => (Results<TIResult1, TIResult2, TErrorIResult>)resultErrorHandler(error, context));

    public static Results<TIResult1, TIResult2, TErrorIResult, BadRequest<ProblemDetails>> ToWebApi<TIResult1, TIResult2, TErrorIResult>(
        this Result<Results<TIResult1, TIResult2>> result,
        HttpContext context,
        Func<IResultError, HttpContext, Option<TErrorIResult>> resultErrorHandler)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TErrorIResult : IResult =>
        result.ToWebApi(context,
                        (error, _) => resultErrorHandler(error, context)
                                      .Map(x => (Results<TErrorIResult, BadRequest<ProblemDetails>>)x)
                                      .GetValueOrDefault(() => error.ToWebApiBadRequest(context)));

    public static Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2, BadRequest<ProblemDetails>> ToWebApi<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2>(
        this Result<Results<TIResult1, TIResult2>> result,
        HttpContext context,
        Func<IResultError, HttpContext, Option<TErrorIResult1>> resultErrorHandler1,
        Func<IResultError, HttpContext, Option<TErrorIResult2>> resultErrorHandler2)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TErrorIResult1 : IResult
        where TErrorIResult2 : IResult =>
        result.Switch(value =>
                      {
                          return value.Result switch
                          {
                              TIResult1 valueResult1 => (Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2, BadRequest<ProblemDetails>>)valueResult1,
                              TIResult2 valueResult2 => (Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2, BadRequest<ProblemDetails>>)valueResult2,
                              _                      => throw new InvalidOperationException()
                          };
                      },
                      error => resultErrorHandler1(error, context).Map(x => (Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2, BadRequest<ProblemDetails>>)x)
                                                                  .OrElse(() => resultErrorHandler2(error, context)
                                                                              .Map(x => (Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2, BadRequest<ProblemDetails>>)x))
                                                                  .GetValueOrDefault(() => error.ToWebApiBadRequest(context)));

    public static Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2, TErrorIResult3, BadRequest<ProblemDetails>> ToWebApi<
        TIResult1, TIResult2, TErrorIResult1, TErrorIResult2, TErrorIResult3>(
        this Result<Results<TIResult1, TIResult2>> result,
        HttpContext context,
        Func<IResultError, HttpContext, Option<TErrorIResult1>> resultErrorHandler1,
        Func<IResultError, HttpContext, Option<TErrorIResult2>> resultErrorHandler2,
        Func<IResultError, HttpContext, Option<TErrorIResult3>> resultErrorHandler3)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TErrorIResult1 : IResult
        where TErrorIResult2 : IResult
        where TErrorIResult3 : IResult =>
        result.Switch(value =>
                      {
                          return value.Result switch
                          {
                              TIResult1 valueResult1 => (Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2, TErrorIResult3, BadRequest<ProblemDetails>>)valueResult1,
                              TIResult2 valueResult2 => (Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2, TErrorIResult3, BadRequest<ProblemDetails>>)valueResult2,
                              _                      => throw new InvalidOperationException()
                          };
                      },
                      error => resultErrorHandler1(error, context).Map(x => (Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2, TErrorIResult3, BadRequest<ProblemDetails>>)x)
                                                                  .OrElse(() => resultErrorHandler2(error, context)
                                                                              .Map(x => (Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2, TErrorIResult3, BadRequest<ProblemDetails>>)x))
                                                                  .OrElse(() => resultErrorHandler3(error, context)
                                                                              .Map(x => (Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2, TErrorIResult3, BadRequest<ProblemDetails>>)x))
                                                                  .GetValueOrDefault(() => error.ToWebApiBadRequest(context)));

    public static Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2> ToWebApi<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2>(
        this Result<Results<TIResult1, TIResult2>> result,
        HttpContext context,
        Func<IResultError, HttpContext, Results<TErrorIResult1, TErrorIResult2>> errorFunc)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TErrorIResult1 : IResult
        where TErrorIResult2 : IResult =>
        result.Switch(value =>
                      {
                          return value.Result switch
                          {
                              TIResult1 valueResult1 => (Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2>)valueResult1,
                              TIResult2 valueResult2 => (Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2>)valueResult2,
                              _                      => throw new InvalidOperationException()
                          };
                      },
                      error =>
                      {
                          return errorFunc(error, context).Result switch
                          {
                              TErrorIResult1 errorResult1 => (Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2>)errorResult1,
                              TErrorIResult2 errorResult2 => (Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2>)errorResult2,
                              _                           => throw new InvalidOperationException()
                          };
                      });

    public static Task<Results<TIResult1, TIResult2, BadRequest<ProblemDetails>>> ToWebApiAsync<TIResult1, TIResult2>(this Task<Result<Results<TIResult1, TIResult2>>> resultTask,
                                                                                                                      HttpContext context)
        where TIResult1 : IResult
        where TIResult2 : IResult =>
        resultTask.ToWebApiAsync(context, ToWebApiBadRequest);

    public static Task<Results<TIResult1, TIResult2, TErrorIResult>> ToWebApiAsync<TIResult1, TIResult2, TErrorIResult>(this Task<Result<Results<TIResult1, TIResult2>>> resultTask,
                                                                                                                        HttpContext context,
                                                                                                                        Func<IResultError, HttpContext, TErrorIResult> resultErrorHandler)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TErrorIResult : IResult =>
        resultTask.SwitchAsync(value =>
                               {
                                   return value.Result switch
                                   {
                                       TIResult1 valueResult1 => valueResult1,
                                       TIResult2 valueResult2 => valueResult2,
                                       _                      => throw new InvalidOperationException()
                                   };
                               },
                               error => (Results<TIResult1, TIResult2, TErrorIResult>)resultErrorHandler(error, context));

    public static Task<Results<TIResult1, TIResult2, TErrorIResult, BadRequest<ProblemDetails>>> ToWebApiAsync<TIResult1, TIResult2, TErrorIResult>(
        this Task<Result<Results<TIResult1, TIResult2>>> resultTask,
        HttpContext context,
        Func<IResultError, HttpContext, Option<TErrorIResult>> resultErrorHandler)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TErrorIResult : IResult =>
        resultTask.ToWebApiAsync(context,
                                 (error, _) => resultErrorHandler(error, context)
                                               .Map(x => (Results<TErrorIResult, BadRequest<ProblemDetails>>)x)
                                               .GetValueOrDefault(() => error.ToWebApiBadRequest(context)));


    public static Task<Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2, BadRequest<ProblemDetails>>> ToWebApiAsync<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2>(
        this Task<Result<Results<TIResult1, TIResult2>>> result,
        HttpContext context,
        Func<IResultError, HttpContext, Option<TErrorIResult1>> resultErrorHandler1,
        Func<IResultError, HttpContext, Option<TErrorIResult2>> resultErrorHandler2)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TErrorIResult1 : IResult
        where TErrorIResult2 : IResult =>
        result.SwitchAsync(value =>
                           {
                               return value.Result switch
                               {
                                   TIResult1 valueResult1 => (Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2, BadRequest<ProblemDetails>>)valueResult1,
                                   TIResult2 valueResult2 => (Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2, BadRequest<ProblemDetails>>)valueResult2,
                                   _                      => throw new InvalidOperationException()
                               };
                           },
                           error => resultErrorHandler1(error, context).Map(x => (Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2, BadRequest<ProblemDetails>>)x)
                                                                       .OrElse(() => resultErrorHandler2(error, context)
                                                                                   .Map(x => (Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2, BadRequest<ProblemDetails>>)x))
                                                                       .GetValueOrDefault(() => error.ToWebApiBadRequest(context)));

    public static Task<Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2, TErrorIResult3, BadRequest<ProblemDetails>>> ToWebApiAsync<
        TIResult1, TIResult2, TErrorIResult1, TErrorIResult2, TErrorIResult3>(
        this Task<Result<Results<TIResult1, TIResult2>>> result,
        HttpContext context,
        Func<IResultError, HttpContext, Option<TErrorIResult1>> resultErrorHandler1,
        Func<IResultError, HttpContext, Option<TErrorIResult2>> resultErrorHandler2,
        Func<IResultError, HttpContext, Option<TErrorIResult3>> resultErrorHandler3)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TErrorIResult1 : IResult
        where TErrorIResult2 : IResult
        where TErrorIResult3 : IResult =>
        result.SwitchAsync(value =>
                           {
                               return value.Result switch
                               {
                                   TIResult1 valueResult1 => (Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2, TErrorIResult3, BadRequest<ProblemDetails>>)valueResult1,
                                   TIResult2 valueResult2 => (Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2, TErrorIResult3, BadRequest<ProblemDetails>>)valueResult2,
                                   _                      => throw new InvalidOperationException()
                               };
                           },
                           error => resultErrorHandler1(error, context).Map(x => (Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2, TErrorIResult3, BadRequest<ProblemDetails>>)x)
                                                                       .OrElse(() => resultErrorHandler2(error, context)
                                                                                   .Map(x => (Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2, TErrorIResult3, BadRequest<ProblemDetails>>)
                                                                                            x))
                                                                       .OrElse(() => resultErrorHandler3(error, context)
                                                                                   .Map(x => (Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2, TErrorIResult3, BadRequest<ProblemDetails>>)
                                                                                            x))
                                                                       .GetValueOrDefault(() => error.ToWebApiBadRequest(context)));

    public static Task<Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2>> ToWebApiAsync<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2>(
        this Task<Result<Results<TIResult1, TIResult2>>> resultTask,
        HttpContext context,
        Func<IResultError, HttpContext, Results<TErrorIResult1, TErrorIResult2>> errorFunc)
        where TIResult1 : IResult
        where TIResult2 : IResult
        where TErrorIResult1 : IResult
        where TErrorIResult2 : IResult =>
        resultTask.SwitchAsync(value =>
                               {
                                   return value.Result switch
                                   {
                                       TIResult1 valueResult1 => (Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2>)valueResult1,
                                       TIResult2 valueResult2 => (Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2>)valueResult2,
                                       _                      => throw new InvalidOperationException()
                                   };
                               },
                               error =>
                               {
                                   return errorFunc(error, context).Result switch
                                   {
                                       TErrorIResult1 errorResult1 => (Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2>)errorResult1,
                                       TErrorIResult2 errorResult2 => (Results<TIResult1, TIResult2, TErrorIResult1, TErrorIResult2>)errorResult2,
                                       _                           => throw new InvalidOperationException()
                                   };
                               });
}