using System.Net;

using FluentAssertions;

using Futurum.Core.Result;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.Micro.Tests;

public class WebApiResultsExtensionsToWebApiResults3Tests
{
    private const string ERROR_MESSAGE = "ErrorMessage";
    private const string REQUEST_PATH = "/RequestPath";

    private const string Property1 = "Property1";
    private const string Property2 = "Property2";

    private const string ErrorMessage1 = "ErrorMessage1";
    private const string ErrorMessage2 = "ErrorMessage2";
    private const string ErrorMessage3 = "ErrorMessage3";

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
                var resultsOk = (Results<Ok, NotFound, Accepted>)TypedResults.Ok();

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext());

                ValidateOk(results);
            }

            [Fact]
            public void result2_success()
            {
                var resultsOk = (Results<Ok, NotFound, Accepted>)TypedResults.NotFound();

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext());

                ValidateNotFound(results);
            }

            [Fact]
            public void result3_success()
            {
                var resultsOk = (Results<Ok, NotFound, Accepted>)TypedResults.Accepted(VALUE);

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext());

                ValidateAccepted(results);
            }

            [Fact]
            public void result_failure()
            {
                var results = Result.Fail<Results<Ok, NotFound, Accepted>>(ERROR_MESSAGE).ToWebApi(CreateHttpContext());

                ValidateBadRequest(results);
            }
        }

        public class Async
        {
            [Fact]
            public async Task result1_success()
            {
                var resultsOk = (Results<Ok, NotFound, Accepted>)TypedResults.Ok();

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext());

                ValidateOk(results);
            }

            [Fact]
            public async Task result2_success()
            {
                var resultsOk = (Results<Ok, NotFound, Accepted>)TypedResults.NotFound();

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext());

                ValidateNotFound(results);
            }

            [Fact]
            public async Task result3_success()
            {
                var resultsOk = (Results<Ok, NotFound, Accepted>)TypedResults.Accepted(VALUE);

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext());

                ValidateAccepted(results);
            }

            [Fact]
            public async Task result_failure()
            {
                var results = await Result.FailAsync<Results<Ok, NotFound, Accepted>>(ERROR_MESSAGE).ToWebApiAsync(CreateHttpContext());

                ValidateBadRequest(results);
            }
        }

        private static void ValidateBadRequest(Results<Ok, NotFound, Accepted, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
            results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be(ERROR_MESSAGE);
            results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
        }

        private static void ValidateNotFound(Results<Ok, NotFound, Accepted, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<NotFound>();
        }

        private static void ValidateAccepted(Results<Ok, NotFound, Accepted, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Accepted>();
        }

        private static void ValidateOk(Results<Ok, NotFound, Accepted, BadRequest<ProblemDetails>> results)
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
                var resultsOk = (Results<Ok, Created, Accepted>)TypedResults.Ok();

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext(), CreateNotFoundFailure);

                ValidateOk(results);
            }

            [Fact]
            public void result2_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted>)TypedResults.Created(VALUE);

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext(), CreateNotFoundFailure);

                ValidateCreated(results);
            }

            [Fact]
            public void result3_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted>)TypedResults.Accepted(VALUE);

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext(), CreateNotFoundFailure);

                ValidateAccepted(results);
            }

            [Fact]
            public void result_failure()
            {
                var resultError = ResultErrorKeyNotFound.Create(KEY, SOURCE_DESCRIPTION);

                var results = Result.Fail<Results<Ok, Created, Accepted>>(resultError).ToWebApi(CreateHttpContext(), CreateNotFoundFailure);

                ValidateNotFound(results);
            }
        }

        public class Async
        {
            [Fact]
            public async Task result1_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted>)TypedResults.Ok();

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext(), CreateNotFoundFailure);

                ValidateOk(results);
            }

            [Fact]
            public async Task result2_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted>)TypedResults.Created(VALUE);

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext(), CreateNotFoundFailure);

                ValidateCreated(results);
            }

            [Fact]
            public async Task result3_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted>)TypedResults.Accepted(VALUE);

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext(), CreateNotFoundFailure);

                ValidateAccepted(results);
            }

            [Fact]
            public async Task result_failure()
            {
                var resultError = ResultErrorKeyNotFound.Create(KEY, SOURCE_DESCRIPTION);

                var results = await Result.FailAsync<Results<Ok, Created, Accepted>>(resultError).ToWebApiAsync(CreateHttpContext(), CreateNotFoundFailure);

                ValidateNotFound(results);
            }
        }
        
        private static NotFound<ProblemDetails> CreateNotFoundFailure(IResultError error, HttpContext context)
        {
            var problemDetails = error.ToProblemDetails(HttpStatusCode.NotFound, context);
            return TypedResults.NotFound(problemDetails);
        }

        private static void ValidateCreated(Results<Ok, Created, Accepted, NotFound<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Created>();
            results.Result.As<Created>().Location.Should().Be(VALUE);
        }

        private static void ValidateNotFound(Results<Ok, Created, Accepted, NotFound<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<NotFound<ProblemDetails>>();
            results.Result.As<NotFound<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            results.Result.As<NotFound<ProblemDetails>>().Value.Detail.Should().Be($"Unable to find key : '{KEY}' in source : '{SOURCE_DESCRIPTION}'");
        }

        private static void ValidateAccepted(Results<Ok, Created, Accepted, NotFound<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Accepted>();
        }

        private static void ValidateOk(Results<Ok, Created, Accepted, NotFound<ProblemDetails>> results)
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
                var resultsOk = (Results<Ok, Created, Accepted>)TypedResults.Ok();

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext(), ToNotFound);

                ValidateOk(results);
            }

            [Fact]
            public void result2_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted>)TypedResults.Created(VALUE);

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext(), ToNotFound);

                ValidateCreated(results);
            }

            [Fact]
            public void result3_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted>)TypedResults.Accepted(VALUE);

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext(), ToNotFound);

                ValidateAccepted(results);
            }

            [Fact]
            public void result_failure_BadRequest()
            {
                var results = Result.Fail<Results<Ok, Created, Accepted>>(ERROR_MESSAGE).ToWebApi(CreateHttpContext(), ToNotFound);

                ValidateBadRequest(results);
            }

            [Fact]
            public void result_failure_NotFound()
            {
                var resultError = ResultErrorKeyNotFound.Create(KEY, SOURCE_DESCRIPTION);

                var results = Result.Fail<Results<Ok, Created, Accepted>>(resultError).ToWebApi(CreateHttpContext(), ToNotFound);

                ValidateNotFound(results);
            }
        }

        public class Async
        {
            [Fact]
            public async Task result1_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted>)TypedResults.Ok();

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext(), ToNotFound);

                ValidateOk(results);
            }

            [Fact]
            public async Task result2_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted>)TypedResults.Created(VALUE);

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext(), ToNotFound);

                ValidateCreated(results);
            }

            [Fact]
            public async Task result3_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted>)TypedResults.Accepted(VALUE);

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext(), ToNotFound);

                ValidateAccepted(results);
            }

            [Fact]
            public async Task result_failure_BadRequest()
            {
                var results = await Result.FailAsync<Results<Ok, Created, Accepted>>(ERROR_MESSAGE).ToWebApiAsync(CreateHttpContext(), ToNotFound);

                ValidateBadRequest(results);
            }

            [Fact]
            public async Task result_failure_NotFound()
            {
                var resultError = ResultErrorKeyNotFound.Create(KEY, SOURCE_DESCRIPTION);

                var results = await Result.FailAsync<Results<Ok, Created, Accepted>>(resultError).ToWebApiAsync(CreateHttpContext(), ToNotFound);

                ValidateNotFound(results);
            }
        }
        
        private static void ValidateBadRequest(Results<Ok, Created, Accepted, NotFound<ProblemDetails>, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
            results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be(ERROR_MESSAGE);
            results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
        }

        private static void ValidateCreated(Results<Ok, Created, Accepted, NotFound<ProblemDetails>, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Created>();
            results.Result.As<Created>().Location.Should().Be(VALUE);
        }

        private static void ValidateNotFound(Results<Ok, Created, Accepted, NotFound<ProblemDetails>, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<NotFound<ProblemDetails>>();
            results.Result.As<NotFound<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            results.Result.As<NotFound<ProblemDetails>>().Value.Detail.Should().Be($"Unable to find key : '{KEY}' in source : '{SOURCE_DESCRIPTION}'");
        }

        private static void ValidateAccepted(Results<Ok, Created, Accepted, NotFound<ProblemDetails>, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Accepted>();
        }

        private static void ValidateOk(Results<Ok, Created, Accepted, NotFound<ProblemDetails>, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Ok>();
        }
    }

    public class ResultErrorHandler2_BadRequest
    {
        public class Sync
        {
            [Fact]
            public void result1_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted>)TypedResults.Ok();

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext(), ToNotFound, ToValidationProblem);

                ValidateOk(results);
            }

            [Fact]
            public void result2_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted>)TypedResults.Created(VALUE);

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext(), ToNotFound, ToValidationProblem);

                ValidateCreated(results);
            }

            [Fact]
            public void result3_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted>)TypedResults.Accepted(VALUE);

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext(), ToNotFound, ToValidationProblem);

                ValidateAccepted(results);
            }

            [Fact]
            public void result_failure_BadRequest()
            {
                var results = Result.Fail<Results<Ok, Created, Accepted>>(ERROR_MESSAGE).ToWebApi(CreateHttpContext(), ToNotFound, ToValidationProblem);

                ValidateBadRequest(results);
            }

            [Fact]
            public void result_failure_NotFound()
            {
                var resultError = ResultErrorKeyNotFound.Create(KEY, SOURCE_DESCRIPTION);

                var results = Result.Fail<Results<Ok, Created, Accepted>>(resultError).ToWebApi(CreateHttpContext(), ToNotFound, ToValidationProblem);

                ValidateNotFound(results);
            }

            [Fact]
            public void result_failure_ValidationProblem()
            {
                var resultError = CreateResultErrorValidation();

                var results = Result.Fail<Results<Ok, Created, Accepted>>(resultError).ToWebApi(CreateHttpContext(), ToNotFound, ToValidationProblem);

                ValidateValidationProblem(results);
            }
        }

        public class Async
        {
            [Fact]
            public async Task result1_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted>)TypedResults.Ok();

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext(), ToNotFound, ToValidationProblem);

                ValidateOk(results);
            }

            [Fact]
            public async Task result2_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted>)TypedResults.Created(VALUE);

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext(), ToNotFound, ToValidationProblem);

                ValidateCreated(results);
            }

            [Fact]
            public async Task result3_success()
            {
                var resultsOk = (Results<Ok, Created, Accepted>)TypedResults.Accepted(VALUE);

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext(), ToNotFound, ToValidationProblem);

                ValidateAccepted(results);
            }

            [Fact]
            public async Task result_failure_BadRequest()
            {
                var results = await Result.FailAsync<Results<Ok, Created, Accepted>>(ERROR_MESSAGE).ToWebApiAsync(CreateHttpContext(), ToNotFound, ToValidationProblem);

                ValidateBadRequest(results);
            }

            [Fact]
            public async Task result_failure_NotFound()
            {
                var resultError = ResultErrorKeyNotFound.Create(KEY, SOURCE_DESCRIPTION);

                var results = await Result.FailAsync<Results<Ok, Created, Accepted>>(resultError).ToWebApiAsync(CreateHttpContext(), ToNotFound, ToValidationProblem);

                ValidateNotFound(results);
            }

            [Fact]
            public async Task result_failure_ValidationProblem()
            {
                var resultError = CreateResultErrorValidation();

                var results = await Result.FailAsync<Results<Ok, Created, Accepted>>(resultError).ToWebApiAsync(CreateHttpContext(), ToNotFound, ToValidationProblem);

                ValidateValidationProblem(results);
            }
        }
        
        private static void ValidateBadRequest(Results<Ok, Created, Accepted, NotFound<ProblemDetails>, ValidationProblem, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
            results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be(ERROR_MESSAGE);
            results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
        }

        private static void ValidateCreated(Results<Ok, Created, Accepted, NotFound<ProblemDetails>, ValidationProblem, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Created>();
            results.Result.As<Created>().Location.Should().Be(VALUE);
        }

        private static void ValidateNotFound(Results<Ok, Created, Accepted, NotFound<ProblemDetails>, ValidationProblem, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<NotFound<ProblemDetails>>();
            results.Result.As<NotFound<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            results.Result.As<NotFound<ProblemDetails>>().Value.Detail.Should().Be($"Unable to find key : '{KEY}' in source : '{SOURCE_DESCRIPTION}'");
        }

        private static void ValidateAccepted(Results<Ok, Created, Accepted, NotFound<ProblemDetails>, ValidationProblem, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Accepted>();
        }
        
        private static void ValidateValidationProblem(Results<Ok, Created, Accepted, NotFound<ProblemDetails>, ValidationProblem, BadRequest<ProblemDetails>> results)
        {
            var errors = new Dictionary<string, string[]>
            {
                { Property1, new[] { ErrorMessage1, ErrorMessage3 } },
                { Property2, new[] { ErrorMessage2 } }
            };

            results.Result.Should().BeOfType<ValidationProblem>();
            results.Result.As<ValidationProblem>().ProblemDetails.Instance.Should().Be(REQUEST_PATH);
            results.Result.As<ValidationProblem>().ProblemDetails.As<HttpValidationProblemDetails>().Errors.Should().BeEquivalentTo(errors);
        }

        private static void ValidateOk(Results<Ok, Created, Accepted, NotFound<ProblemDetails>, ValidationProblem, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Ok>();
        }
    }

    private static DefaultHttpContext CreateHttpContext() =>
        new() { Request = { Path = REQUEST_PATH } };

    private static ResultErrorValidation CreateResultErrorValidation()
    {
        var validationErrors = new List<ValidationError>
        {
            new(Property1, new[] { ErrorMessage1, ErrorMessage3 }),
            new(Property2, new[] { ErrorMessage2 })
        };

        return validationErrors.ToResultError();
    }
}