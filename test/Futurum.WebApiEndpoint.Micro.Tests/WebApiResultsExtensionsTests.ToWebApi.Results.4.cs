using System.Net;

using FluentAssertions;

using Futurum.Core.Result;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.Micro.Tests;

public class WebApiResultsExtensionsToWebApiResults4Tests
{
    private const string ERROR_MESSAGE = "ErrorMessage";
    private const string REQUEST_PATH = "/RequestPath";

    private const string VALUE = "Value";

    private const string KEY = "KEY";
    private const string SOURCE_DESCRIPTION = "Source Description";

    public class ResultErrorHandler0
    {
        public class Sync
        {
            [Fact]
            public void result1_success()
            {
                var resultsOk = (Results<Ok, NotFound, Accepted, Created>)TypedResults.Ok();

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext());

                ValidateOk(results);
            }

            [Fact]
            public void result2_success()
            {
                var resultsOk = (Results<Ok, NotFound, Accepted, Created>)TypedResults.NotFound();

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext());

                ValidateNotFound(results);
            }

            [Fact]
            public void result3_success()
            {
                var resultsOk = (Results<Ok, NotFound, Accepted, Created>)TypedResults.Accepted(VALUE);

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext());

                ValidateAccepted(results);
            }

            [Fact]
            public void result4_success()
            {
                var resultsOk = (Results<Ok, NotFound, Accepted, Created>)TypedResults.Created(VALUE);

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext());

                ValidateCreated(results);
            }

            [Fact]
            public void result_failure()
            {
                var results = Result.Fail<Results<Ok, NotFound, Accepted, Created>>(ERROR_MESSAGE).ToWebApi(CreateHttpContext());

                ValidateBadRequest(results);
            }
        }

        public class Async
        {
            [Fact]
            public async Task result1_success()
            {
                var resultsOk = (Results<Ok, NotFound, Accepted, Created>)TypedResults.Ok();

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext());

                ValidateOk(results);
            }

            [Fact]
            public async Task result2_success()
            {
                var resultsOk = (Results<Ok, NotFound, Accepted, Created>)TypedResults.NotFound();

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext());

                ValidateNotFound(results);
            }

            [Fact]
            public async Task result3_success()
            {
                var resultsOk = (Results<Ok, NotFound, Accepted, Created>)TypedResults.Accepted(VALUE);

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext());

                ValidateAccepted(results);
            }

            [Fact]
            public async Task result4_success()
            {
                var resultsOk = (Results<Ok, NotFound, Accepted, Created>)TypedResults.Created(VALUE);

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext());

                ValidateCreated(results);
            }

            [Fact]
            public async Task result_failure()
            {
                var results = await Result.FailAsync<Results<Ok, NotFound, Accepted, Created>>(ERROR_MESSAGE).ToWebApiAsync(CreateHttpContext());

                ValidateBadRequest(results);
            }
        }

        private static void ValidateBadRequest(Results<Ok, NotFound, Accepted, Created, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
            results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be(ERROR_MESSAGE);
            results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
        }

        private static void ValidateNotFound(Results<Ok, NotFound, Accepted, Created, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<NotFound>();
        }

        private static void ValidateAccepted(Results<Ok, NotFound, Accepted, Created, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Accepted>();
        }

        private static void ValidateCreated(Results<Ok, NotFound, Accepted, Created, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Created>();
        }

        private static void ValidateOk(Results<Ok, NotFound, Accepted, Created, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Ok>();
        }
    }

    public class ResultErrorHandler1
    {
        public class Sync
        {
            [Fact]
            public void result1_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted, ContentHttpResult>)TypedResults.Ok();

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext(), CreateNotFoundFailure);

                ValidateOk(results);
            }

            [Fact]
            public void result2_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted, ContentHttpResult>)TypedResults.Created(VALUE);

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext(), CreateNotFoundFailure);

                ValidateCreated(results);
            }

            [Fact]
            public void result3_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted, ContentHttpResult>)TypedResults.Accepted(VALUE);

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext(), CreateNotFoundFailure);

                ValidateAccepted(results);
            }

            [Fact]
            public void result4_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted, ContentHttpResult>)TypedResults.Text(VALUE);

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext(), CreateNotFoundFailure);

                ValidateContentHttpResult(results);
            }

            [Fact]
            public void result_failure()
            {
                var resultError = ResultErrorKeyNotFound.Create(KEY, SOURCE_DESCRIPTION);

                var results = Result.Fail<Results<Ok, Created, Accepted, ContentHttpResult>>(resultError).ToWebApi(CreateHttpContext(), CreateNotFoundFailure);

                ValidateNotFound(results);
            }
        }

        public class Async
        {
            [Fact]
            public async Task result1_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted, ContentHttpResult>)TypedResults.Ok();

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext(), CreateNotFoundFailure);

                ValidateOk(results);
            }

            [Fact]
            public async Task result2_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted, ContentHttpResult>)TypedResults.Created(VALUE);

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext(), CreateNotFoundFailure);

                ValidateCreated(results);
            }

            [Fact]
            public async Task result3_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted, ContentHttpResult>)TypedResults.Accepted(VALUE);

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext(), CreateNotFoundFailure);

                ValidateAccepted(results);
            }

            [Fact]
            public async Task result4_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted, ContentHttpResult>)TypedResults.Text(VALUE);

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext(), CreateNotFoundFailure);

                ValidateContentHttpResult(results);
            }

            [Fact]
            public async Task result_failure()
            {
                var resultError = ResultErrorKeyNotFound.Create(KEY, SOURCE_DESCRIPTION);

                var results = await Result.FailAsync<Results<Ok, Created, Accepted, ContentHttpResult>>(resultError).ToWebApiAsync(CreateHttpContext(), CreateNotFoundFailure);

                ValidateNotFound(results);
            }
        }
        
        private static NotFound<ProblemDetails> CreateNotFoundFailure(IResultError error, HttpContext context)
        {
            var problemDetails = error.ToProblemDetails(HttpStatusCode.NotFound, context);
            return TypedResults.NotFound(problemDetails);
        }

        private static void ValidateCreated(Results<Ok, Created, Accepted, ContentHttpResult, NotFound<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Created>();
            results.Result.As<Created>().Location.Should().Be(VALUE);
        }

        private static void ValidateNotFound(Results<Ok, Created, Accepted, ContentHttpResult, NotFound<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<NotFound<ProblemDetails>>();
            results.Result.As<NotFound<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            results.Result.As<NotFound<ProblemDetails>>().Value.Detail.Should().Be($"Unable to find key : '{KEY}' in source : '{SOURCE_DESCRIPTION}'");
        }

        private static void ValidateAccepted(Results<Ok, Created, Accepted, ContentHttpResult, NotFound<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Accepted>();
        }

        private static void ValidateContentHttpResult(Results<Ok, Created, Accepted, ContentHttpResult, NotFound<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<ContentHttpResult>();
            results.Result.As<ContentHttpResult>().ResponseContent.Should().Be(VALUE);
        }

        private static void ValidateOk(Results<Ok, Created, Accepted, ContentHttpResult, NotFound<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Ok>();
        }
    }

    public class ResultErrorHandler1_BadRequest
    {
        public class Sync
        {
            [Fact]
            public void result1_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted, ContentHttpResult>)TypedResults.Ok();

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext(), ToNotFound);

                ValidateOk(results);
            }

            [Fact]
            public void result2_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted, ContentHttpResult>)TypedResults.Created(VALUE);

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext(), ToNotFound);

                ValidateCreated(results);
            }

            [Fact]
            public void result3_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted, ContentHttpResult>)TypedResults.Accepted(VALUE);

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext(), ToNotFound);

                ValidateAccepted(results);
            }

            [Fact]
            public void result4_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted, ContentHttpResult>)TypedResults.Text(VALUE);

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext(), ToNotFound);

                ValidateContentHttpResult(results);
            }

            [Fact]
            public void result_failure_BadRequest()
            {
                var results = Result.Fail<Results<Ok, Created, Accepted, ContentHttpResult>>(ERROR_MESSAGE).ToWebApi(CreateHttpContext(), ToNotFound);

                ValidateBadRequest(results);
            }

            [Fact]
            public void result_failure_NotFound()
            {
                var resultError = ResultErrorKeyNotFound.Create(KEY, SOURCE_DESCRIPTION);

                var results = Result.Fail<Results<Ok, Created, Accepted, ContentHttpResult>>(resultError).ToWebApi(CreateHttpContext(), ToNotFound);

                ValidateNotFound(results);
            }
        }

        public class Async
        {
            [Fact]
            public async Task result1_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted, ContentHttpResult>)TypedResults.Ok();

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext(), ToNotFound);

                ValidateOk(results);
            }

            [Fact]
            public async Task result2_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted, ContentHttpResult>)TypedResults.Created(VALUE);

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext(), ToNotFound);

                ValidateCreated(results);
            }

            [Fact]
            public async Task result3_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted, ContentHttpResult>)TypedResults.Accepted(VALUE);

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext(), ToNotFound);

                ValidateAccepted(results);
            }

            [Fact]
            public async Task result4_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted, ContentHttpResult>)TypedResults.Text(VALUE);

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext(), ToNotFound);

                ValidateContentHttpResult(results);
            }

            [Fact]
            public async Task result_failure_BadRequest()
            {
                var results = await Result.FailAsync<Results<Ok, Created, Accepted, ContentHttpResult>>(ERROR_MESSAGE).ToWebApiAsync(CreateHttpContext(), ToNotFound);

                ValidateBadRequest(results);
            }

            [Fact]
            public async Task result_failure_NotFound()
            {
                var resultError = ResultErrorKeyNotFound.Create(KEY, SOURCE_DESCRIPTION);

                var results = await Result.FailAsync<Results<Ok, Created, Accepted, ContentHttpResult>>(resultError).ToWebApiAsync(CreateHttpContext(), ToNotFound);

                ValidateNotFound(results);
            }
        }
        
        private static void ValidateBadRequest(Results<Ok, Created, Accepted, ContentHttpResult, NotFound<ProblemDetails>, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
            results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be(ERROR_MESSAGE);
            results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
        }

        private static void ValidateCreated(Results<Ok, Created, Accepted, ContentHttpResult, NotFound<ProblemDetails>, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Created>();
            results.Result.As<Created>().Location.Should().Be(VALUE);
        }

        private static void ValidateNotFound(Results<Ok, Created, Accepted, ContentHttpResult, NotFound<ProblemDetails>, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<NotFound<ProblemDetails>>();
            results.Result.As<NotFound<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            results.Result.As<NotFound<ProblemDetails>>().Value.Detail.Should().Be($"Unable to find key : '{KEY}' in source : '{SOURCE_DESCRIPTION}'");
        }

        private static void ValidateAccepted(Results<Ok, Created, Accepted, ContentHttpResult, NotFound<ProblemDetails>, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Accepted>();
        }

        private static void ValidateContentHttpResult(Results<Ok, Created, Accepted, ContentHttpResult, NotFound<ProblemDetails>, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<ContentHttpResult>();
            results.Result.As<ContentHttpResult>().ResponseContent.Should().Be(VALUE);
        }

        private static void ValidateOk(Results<Ok, Created, Accepted, ContentHttpResult, NotFound<ProblemDetails>, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Ok>();
        }
    }

    private static DefaultHttpContext CreateHttpContext() =>
        new() { Request = { Path = REQUEST_PATH } };
}