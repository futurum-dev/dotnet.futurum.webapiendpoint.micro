using System.Net;

using FluentAssertions;

using Futurum.Core.Result;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.Micro.Tests;

public class WebApiResultsExtensionsToWebApiResults5Tests
{
    private const string ERROR_MESSAGE = "ErrorMessage";
    private const string REQUEST_PATH = "/RequestPath";

    private const string VALUE = "Value";

    public class ResultErrorHandler0
    {
        public class Sync
        {
            [Fact]
            public void result1_success()
            {
                var resultsOk = (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.Ok();

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext());

                ValidateOk(results);
            }

            [Fact]
            public void result2_success()
            {
                var resultsOk = (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.NotFound();

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext());

                ValidateNotFound(results);
            }

            [Fact]
            public void result3_success()
            {
                var resultsOk = (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.Accepted(VALUE);

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext());

                ValidateAccepted(results);
            }

            [Fact]
            public void result4_success()
            {
                var resultsOk = (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.Created(VALUE);

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext());

                ValidateCreated(results);
            }

            [Fact]
            public void result5_success()
            {
                var resultsOk = (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.Conflict();

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext());

                ValidateConflict(results);
            }

            [Fact]
            public void result_failure()
            {
                var results = Result.Fail<Results<Ok, NotFound, Accepted, Created, Conflict>>(ERROR_MESSAGE).ToWebApi(CreateHttpContext());

                ValidateBadRequest(results);
            }
        }

        public class Async
        {
            [Fact]
            public async Task result1_success()
            {
                var resultsOk = (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.Ok();

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext());

                ValidateOk(results);
            }

            [Fact]
            public async Task result2_success()
            {
                var resultsOk = (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.NotFound();

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext());

                ValidateNotFound(results);
            }

            [Fact]
            public async Task result3_success()
            {
                var resultsOk = (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.Accepted(VALUE);

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext());

                ValidateAccepted(results);
            }

            [Fact]
            public async Task result4_success()
            {
                var resultsOk = (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.Created(VALUE);

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext());

                ValidateCreated(results);
            }

            [Fact]
            public async Task result5_success()
            {
                var resultsOk = (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.Conflict();

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext());

                ValidateConflict(results);
            }

            [Fact]
            public async Task result_failure()
            {
                var results = await Result.FailAsync<Results<Ok, NotFound, Accepted, Created, Conflict>>(ERROR_MESSAGE).ToWebApiAsync(CreateHttpContext());

                ValidateBadRequest(results);
            }
        }

        private static void ValidateBadRequest(Results<Ok, NotFound, Accepted, Created, Conflict, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
            results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be(ERROR_MESSAGE);
            results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
        }

        private static void ValidateNotFound(Results<Ok, NotFound, Accepted, Created, Conflict, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<NotFound>();
        }

        private static void ValidateAccepted(Results<Ok, NotFound, Accepted, Created, Conflict, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Accepted>();
        }

        private static void ValidateCreated(Results<Ok, NotFound, Accepted, Created, Conflict, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Created>();
        }

        private static void ValidateConflict(Results<Ok, NotFound, Accepted, Created, Conflict, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Conflict>();
        }

        private static void ValidateOk(Results<Ok, NotFound, Accepted, Created, Conflict, BadRequest<ProblemDetails>> results)
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
                var resultsOk = (Results<Ok, ContentHttpResult, Accepted, Created, Conflict>)TypedResults.Ok();

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext(), CreateNotFoundFailure);

                ValidateOk(results);
            }

            [Fact]
            public void result2_success()
            {
                var resultsOk = (Results<Ok, ContentHttpResult, Accepted, Created, Conflict>)TypedResults.Text(VALUE);

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext(), CreateNotFoundFailure);

                ValidateContentHttpResult(results);
            }

            [Fact]
            public void result3_success()
            {
                var resultsOk = (Results<Ok, ContentHttpResult, Accepted, Created, Conflict>)TypedResults.Accepted(VALUE);

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext(), CreateNotFoundFailure);

                ValidateAccepted(results);
            }

            [Fact]
            public void result4_success()
            {
                var resultsOk = (Results<Ok, ContentHttpResult, Accepted, Created, Conflict>)TypedResults.Created(VALUE);

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext(), CreateNotFoundFailure);

                ValidateCreated(results);
            }

            [Fact]
            public void result5_success()
            {
                var resultsOk = (Results<Ok, ContentHttpResult, Accepted, Created, Conflict>)TypedResults.Conflict();

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext(), CreateNotFoundFailure);

                ValidateConflict(results);
            }

            [Fact]
            public void result_failure()
            {
                var results = Result.Fail<Results<Ok, ContentHttpResult, Accepted, Created, Conflict>>(ERROR_MESSAGE).ToWebApi(CreateHttpContext(), CreateNotFoundFailure);

                ValidateNotFound(results);
            }
        }

        public class Async
        {
            [Fact]
            public async Task result1_success()
            {
                var resultsOk = (Results<Ok, ContentHttpResult, Accepted, Created, Conflict>)TypedResults.Ok();

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext(), CreateNotFoundFailure);

                ValidateOk(results);
            }

            [Fact]
            public async Task result2_success()
            {
                var resultsOk = (Results<Ok, ContentHttpResult, Accepted, Created, Conflict>)TypedResults.Text(VALUE);

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext(), CreateNotFoundFailure);

                ValidateContentHttpResult(results);
            }

            [Fact]
            public async Task result3_success()
            {
                var resultsOk = (Results<Ok, ContentHttpResult, Accepted, Created, Conflict>)TypedResults.Accepted(VALUE);

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext(), CreateNotFoundFailure);

                ValidateAccepted(results);
            }

            [Fact]
            public async Task result4_success()
            {
                var resultsOk = (Results<Ok, ContentHttpResult, Accepted, Created, Conflict>)TypedResults.Created(VALUE);

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext(), CreateNotFoundFailure);

                ValidateCreated(results);
            }

            [Fact]
            public async Task result5_success()
            {
                var resultsOk = (Results<Ok, ContentHttpResult, Accepted, Created, Conflict>)TypedResults.Conflict();

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext(), CreateNotFoundFailure);

                ValidateConflict(results);
            }

            [Fact]
            public async Task result_failure()
            {
                var results = await Result.FailAsync<Results<Ok, ContentHttpResult, Accepted, Created, Conflict>>(ERROR_MESSAGE).ToWebApiAsync(CreateHttpContext(), CreateNotFoundFailure);

                ValidateNotFound(results);
            }
        }

        private static void ValidateContentHttpResult(Results<Ok, ContentHttpResult, Accepted, Created, Conflict, NotFound<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<ContentHttpResult>();
        }

        private static void ValidateAccepted(Results<Ok, ContentHttpResult, Accepted, Created, Conflict, NotFound<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Accepted>();
        }

        private static void ValidateCreated(Results<Ok, ContentHttpResult, Accepted, Created, Conflict, NotFound<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Created>();
        }

        private static void ValidateConflict(Results<Ok, ContentHttpResult, Accepted, Created, Conflict, NotFound<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Conflict>();
        }

        private static void ValidateNotFound(Results<Ok, ContentHttpResult, Accepted, Created, Conflict, NotFound<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<NotFound<ProblemDetails>>();
        }

        private static void ValidateOk(Results<Ok, ContentHttpResult, Accepted, Created, Conflict, NotFound<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Ok>();
        }

        private static NotFound<ProblemDetails> CreateNotFoundFailure(IResultError error, HttpContext context)
        {
            var problemDetails = error.ToProblemDetails(HttpStatusCode.NotFound, context);
            return TypedResults.NotFound(problemDetails);
        }
    }

    private static DefaultHttpContext CreateHttpContext() =>
        new() { Request = { Path = REQUEST_PATH } };
}