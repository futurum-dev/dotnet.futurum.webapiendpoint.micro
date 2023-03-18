using System.Net;

using FluentAssertions;

using Futurum.Core.Option;
using Futurum.Core.Result;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.Micro.Tests;

public class WebApiResultsExtensionsToWebApiResults1Tests
{
    private const string ERROR_MESSAGE = "ErrorMessage";
    private const string REQUEST_PATH = "/RequestPath";

    private const string Property1 = "Property1";
    private const string Property2 = "Property2";

    private const string ErrorMessage1 = "ErrorMessage1";
    private const string ErrorMessage2 = "ErrorMessage2";
    private const string ErrorMessage3 = "ErrorMessage3";

    private const string KEY = "KEY";
    private const string SOURCE_DESCRIPTION = "Source Description";

    public class ResultErrorHandler0
    {
        public class Sync
        {
            [Fact]
            public void result_success()
            {
                var resultsOk = TypedResults.Ok();

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext());

                ValidateOk(results);
            }

            [Fact]
            public void result_failure()
            {
                var results = Result.Fail<Ok>(ERROR_MESSAGE).ToWebApi(CreateHttpContext());

                ValidateBadRequest(results);
            }
        }

        public class Async
        {
            [Fact]
            public async Task result_success()
            {
                var resultsOk = TypedResults.Ok();

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext());

                ValidateOk(results);
            }

            [Fact]
            public async Task result_failure()
            {
                var results = await Result.FailAsync<Ok>(ERROR_MESSAGE).ToWebApiAsync(CreateHttpContext());

                ValidateBadRequest(results);
            }
        }

        private static void ValidateBadRequest(Results<Ok, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
            results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be(ERROR_MESSAGE);
            results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
        }

        private static void ValidateOk(Results<Ok, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Ok>();
        }
    }

    public class ResultErrorHandler1
    {
        public class Sync
        {
            [Fact]
            public void result_success()
            {
                var resultsOk = TypedResults.Ok();

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext(), CreateNotFoundFailure);

                ValidateOk(results);
            }

            [Fact]
            public void result_failure_NotFound()
            {
                var resultError = ResultErrorKeyNotFound.Create(KEY, SOURCE_DESCRIPTION);

                var results = Result.Fail<Ok>(resultError).ToWebApi(CreateHttpContext(), CreateNotFoundFailure);

                ValidateNotFound(results);
            }
        }

        public class Async
        {
            [Fact]
            public async Task result_success()
            {
                var resultsOk = TypedResults.Ok();

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext(), CreateNotFoundFailure);

                ValidateOk(results);
            }

            [Fact]
            public async Task result_failure_NotFound()
            {
                var resultError = ResultErrorKeyNotFound.Create(KEY, SOURCE_DESCRIPTION);

                var results = await Result.FailAsync<Ok>(resultError).ToWebApiAsync(CreateHttpContext(), CreateNotFoundFailure);

                ValidateNotFound(results);
            }
        }

        private static void ValidateNotFound(Results<Ok, NotFound<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<NotFound<ProblemDetails>>();
            results.Result.As<NotFound<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            results.Result.As<NotFound<ProblemDetails>>().Value.Detail.Should().Be($"Unable to find key : '{KEY}' in source : '{SOURCE_DESCRIPTION}'");
        }

        private static NotFound<ProblemDetails> CreateNotFoundFailure(IResultError error, HttpContext context)
        {
            var problemDetails = error.ToProblemDetails(HttpStatusCode.NotFound, context);
            return TypedResults.NotFound(problemDetails);
        }

        private static void ValidateOk(Results<Ok, NotFound<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Ok>();
        }
    }

    public class ResultErrorHandler1_WithBadRequest
    {
        public class Sync
        {
            [Fact]
            public void result_success()
            {
                var resultsOk = TypedResults.Ok();

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext(), ToNotFound);

                ValidateOk(results);
            }

            [Fact]
            public void result_failure_bad_request()
            {
                var results = Result.Fail<Ok>(ERROR_MESSAGE).ToWebApi(CreateHttpContext(), ToNotFound);

                ValidateBadRequest(results);
            }

            [Fact]
            public void result_failure_not_found()
            {
                var resultError = ResultErrorKeyNotFound.Create(KEY, SOURCE_DESCRIPTION);

                var results = Result.Fail<Ok>(resultError).ToWebApi(CreateHttpContext(), ToNotFound);

                ValidateNotFound(results);
            }
        }

        public class Async
        {
            [Fact]
            public async Task result_success()
            {
                var resultsOk = TypedResults.Ok();

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext(), ToNotFound);

                ValidateOk(results);
            }

            [Fact]
            public async Task result_failure_bad_request()
            {
                var results = await Result.FailAsync<Ok>(ERROR_MESSAGE).ToWebApiAsync(CreateHttpContext(), ToNotFound);

                ValidateBadRequest(results);
            }

            [Fact]
            public async Task result_failure_not_found()
            {
                var resultError = ResultErrorKeyNotFound.Create(KEY, SOURCE_DESCRIPTION);

                var results = await Result.FailAsync<Ok>(resultError).ToWebApiAsync(CreateHttpContext(), ToNotFound);

                ValidateNotFound(results);
            }
        }

        private static void ValidateBadRequest(Results<Ok, NotFound<ProblemDetails>, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
            results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be(ERROR_MESSAGE);
            results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
        }

        private static void ValidateNotFound(Results<Ok, NotFound<ProblemDetails>, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<NotFound<ProblemDetails>>();
            results.Result.As<NotFound<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            results.Result.As<NotFound<ProblemDetails>>().Value.Detail.Should().Be($"Unable to find key : '{KEY}' in source : '{SOURCE_DESCRIPTION}'");
        }

        private static void ValidateOk(Results<Ok, NotFound<ProblemDetails>, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Ok>();
        }
    }

    public class ResultErrorHandler2_WithBadRequest
    {
        public class Sync
        {
            [Fact]
            public void result_success()
            {
                var resultsOk = TypedResults.Ok();

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext(), ToNotFound, ToValidationProblem);

                ValidateOk(results);
            }

            [Fact]
            public void result_failure_BadRequest()
            {
                var results = Result.Fail<Ok>(ERROR_MESSAGE).ToWebApi(CreateHttpContext(), ToNotFound, ToValidationProblem);

                ValidateBadRequest(results);
            }

            [Fact]
            public void result_failure_NotFound()
            {
                var resultError = ResultErrorKeyNotFound.Create(KEY, SOURCE_DESCRIPTION);

                var results = Result.Fail<Ok>(resultError).ToWebApi(CreateHttpContext(), ToNotFound, ToValidationProblem);

                ValidateNotFound(results);
            }

            [Fact]
            public void result_failure_ValidationProblem()
            {
                var resultError = CreateResultErrorValidation();

                var results = Result.Fail<Ok>(resultError).ToWebApi(CreateHttpContext(), ToNotFound, ToValidationProblem);

                ValidateValidationProblem(results);
            }
        }

        public class Async
        {
            [Fact]
            public async Task result_success()
            {
                var resultsOk = TypedResults.Ok();

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext(), ToNotFound, ToValidationProblem);

                ValidateOk(results);
            }

            [Fact]
            public async Task result_failure_BadRequest()
            {
                var results = await Result.FailAsync<Ok>(ERROR_MESSAGE).ToWebApiAsync(CreateHttpContext(), ToNotFound, ToValidationProblem);

                ValidateBadRequest(results);
            }

            [Fact]
            public async Task result_failure_NotFound()
            {
                var resultError = ResultErrorKeyNotFound.Create(KEY, SOURCE_DESCRIPTION);

                var results = await Result.FailAsync<Ok>(resultError).ToWebApiAsync(CreateHttpContext(), ToNotFound, ToValidationProblem);

                ValidateNotFound(results);
            }

            [Fact]
            public async Task result_failure_ValidationProblem()
            {
                var resultError = CreateResultErrorValidation();

                var results = await Result.FailAsync<Ok>(resultError).ToWebApiAsync(CreateHttpContext(), ToNotFound, ToValidationProblem);

                ValidateValidationProblem(results);
            }
        }

        private static void ValidateBadRequest(Results<Ok, NotFound<ProblemDetails>, ValidationProblem, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
            results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be(ERROR_MESSAGE);
            results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
        }

        private static void ValidateNotFound(Results<Ok, NotFound<ProblemDetails>, ValidationProblem, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<NotFound<ProblemDetails>>();
            results.Result.As<NotFound<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            results.Result.As<NotFound<ProblemDetails>>().Value.Detail.Should().Be($"Unable to find key : '{KEY}' in source : '{SOURCE_DESCRIPTION}'");
        }

        private static void ValidateValidationProblem(Results<Ok, NotFound<ProblemDetails>, ValidationProblem, BadRequest<ProblemDetails>> results)
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

        private static void ValidateOk(Results<Ok, NotFound<ProblemDetails>, ValidationProblem, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Ok>();
        }
    }

    public class ResultErrorHandler3_WithBadRequest
    {
        public class Sync
        {
            [Fact]
            public void result_success()
            {
                var resultsOk = TypedResults.Ok();

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext(), ToNotFound, ToValidationProblem, ToConflict);

                ValidateOk(results);
            }

            [Fact]
            public void result_failure_BadRequest()
            {
                var results = Result.Fail<Ok>(ERROR_MESSAGE).ToWebApi(CreateHttpContext(), ToNotFound, ToValidationProblem, ToConflict);

                ValidateBadRequest(results);
            }

            [Fact]
            public void result_failure_NotFound()
            {
                var resultError = ResultErrorKeyNotFound.Create(KEY, SOURCE_DESCRIPTION);

                var results = Result.Fail<Ok>(resultError).ToWebApi(CreateHttpContext(), ToNotFound, ToValidationProblem, ToConflict);

                ValidateNotFound(results);
            }

            [Fact]
            public void result_failure_ValidationProblem()
            {
                var resultError = CreateResultErrorValidation();

                var results = Result.Fail<Ok>(resultError).ToWebApi(CreateHttpContext(), ToNotFound, ToValidationProblem, ToConflict);

                ValidateValidationProblem(results);
            }

            [Fact]
            public void result_failure_Conflict()
            {
                var resultError = CreateResultErrorConflict();

                var results = Result.Fail<Ok>(resultError).ToWebApi(CreateHttpContext(), ToNotFound, ToValidationProblem, ToConflict);

                ValidateConflict(results);
            }
        }

        public class Async
        {
            [Fact]
            public async Task result_success()
            {
                var resultsOk = TypedResults.Ok();

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext(), ToNotFound, ToValidationProblem, ToConflict);

                ValidateOk(results);
            }

            [Fact]
            public async Task result_failure_BadRequest()
            {
                var results = await Result.FailAsync<Ok>(ERROR_MESSAGE).ToWebApiAsync(CreateHttpContext(), ToNotFound, ToValidationProblem, ToConflict);

                ValidateBadRequest(results);
            }

            [Fact]
            public async Task result_failure_NotFound()
            {
                var resultError = ResultErrorKeyNotFound.Create(KEY, SOURCE_DESCRIPTION);

                var results = await Result.FailAsync<Ok>(resultError).ToWebApiAsync(CreateHttpContext(), ToNotFound, ToValidationProblem, ToConflict);

                ValidateNotFound(results);
            }

            [Fact]
            public async Task result_failure_ValidationProblem()
            {
                var resultError = CreateResultErrorValidation();

                var results = await Result.FailAsync<Ok>(resultError).ToWebApiAsync(CreateHttpContext(), ToNotFound, ToValidationProblem, ToConflict);

                ValidateValidationProblem(results);
            }

            [Fact]
            public async Task result_failure_Conflict()
            {
                var resultError = CreateResultErrorConflict();

                var results = await Result.FailAsync<Ok>(resultError).ToWebApiAsync(CreateHttpContext(), ToNotFound, ToValidationProblem, ToConflict);

                ValidateConflict(results);
            }
        }

        private static void ValidateBadRequest(Results<Ok, NotFound<ProblemDetails>, ValidationProblem, Conflict, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
            results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be(ERROR_MESSAGE);
            results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
        }

        private static void ValidateNotFound(Results<Ok, NotFound<ProblemDetails>, ValidationProblem, Conflict, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<NotFound<ProblemDetails>>();
            results.Result.As<NotFound<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            results.Result.As<NotFound<ProblemDetails>>().Value.Detail.Should().Be($"Unable to find key : '{KEY}' in source : '{SOURCE_DESCRIPTION}'");
        }

        private static void ValidateValidationProblem(Results<Ok, NotFound<ProblemDetails>, ValidationProblem, Conflict, BadRequest<ProblemDetails>> results)
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

        private static void ValidateConflict(Results<Ok, NotFound<ProblemDetails>, ValidationProblem, Conflict, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Conflict>();
        }

        private static void ValidateOk(Results<Ok, NotFound<ProblemDetails>, ValidationProblem, Conflict, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Ok>();
        }
    }

    public class ResultErrorHandler4_WithBadRequest
    {
        public class Sync
        {
            [Fact]
            public void result_success()
            {
                var resultsOk = TypedResults.Ok();

                var results = resultsOk.ToResultOk().ToWebApi(CreateHttpContext(), ToNotFound, ToValidationProblem, ToConflict, ToChallenge);

                ValidateOk(results);
            }

            [Fact]
            public void result_failure_BadRequest()
            {
                var results = Result.Fail<Ok>(ERROR_MESSAGE).ToWebApi(CreateHttpContext(), ToNotFound, ToValidationProblem, ToConflict, ToChallenge);

                ValidateBadRequest(results);
            }

            [Fact]
            public void result_failure_NotFound()
            {
                var resultError = ResultErrorKeyNotFound.Create(KEY, SOURCE_DESCRIPTION);

                var results = Result.Fail<Ok>(resultError).ToWebApi(CreateHttpContext(), ToNotFound, ToValidationProblem, ToConflict, ToChallenge);

                ValidateNotFound(results);
            }

            [Fact]
            public void result_failure_ValidationProblem()
            {
                var resultError = CreateResultErrorValidation();

                var results = Result.Fail<Ok>(resultError).ToWebApi(CreateHttpContext(), ToNotFound, ToValidationProblem, ToConflict, ToChallenge);

                ValidateValidationProblem(results);
            }

            [Fact]
            public void result_failure_Conflict()
            {
                var resultError = CreateResultErrorConflict();

                var results = Result.Fail<Ok>(resultError).ToWebApi(CreateHttpContext(), ToNotFound, ToValidationProblem, ToConflict, ToChallenge);

                ValidateConflict(results);
            }

            [Fact]
            public void result_failure_Challenge()
            {
                var resultError = CreateResultErrorChallenge();

                var results = Result.Fail<Ok>(resultError).ToWebApi(CreateHttpContext(), ToNotFound, ToValidationProblem, ToConflict, ToChallenge);

                ValidateChallenge(results);
            }
        }

        public class Async
        {
            [Fact]
            public async Task result_success()
            {
                var resultsOk = TypedResults.Ok();

                var results = await resultsOk.ToResultOkAsync().ToWebApiAsync(CreateHttpContext(), ToNotFound, ToValidationProblem, ToConflict, ToChallenge);

                ValidateOk(results);
            }

            [Fact]
            public async Task result_failure_BadRequest()
            {
                var results = await Result.FailAsync<Ok>(ERROR_MESSAGE).ToWebApiAsync(CreateHttpContext(), ToNotFound, ToValidationProblem, ToConflict, ToChallenge);

                ValidateBadRequest(results);
            }

            [Fact]
            public async Task result_failure_NotFound()
            {
                var resultError = ResultErrorKeyNotFound.Create(KEY, SOURCE_DESCRIPTION);

                var results = await Result.FailAsync<Ok>(resultError).ToWebApiAsync(CreateHttpContext(), ToNotFound, ToValidationProblem, ToConflict, ToChallenge);

                ValidateNotFound(results);
            }

            [Fact]
            public async Task result_failure_ValidationProblem()
            {
                var resultError = CreateResultErrorValidation();

                var results = await Result.FailAsync<Ok>(resultError).ToWebApiAsync(CreateHttpContext(), ToNotFound, ToValidationProblem, ToConflict, ToChallenge);

                ValidateValidationProblem(results);
            }

            [Fact]
            public async Task result_failure_Conflict()
            {
                var resultError = CreateResultErrorConflict();

                var results = await Result.FailAsync<Ok>(resultError).ToWebApiAsync(CreateHttpContext(), ToNotFound, ToValidationProblem, ToConflict, ToChallenge);

                ValidateConflict(results);
            }

            [Fact]
            public async Task result_failure_Challenge()
            {
                var resultError = CreateResultErrorChallenge();

                var results = await Result.FailAsync<Ok>(resultError).ToWebApiAsync(CreateHttpContext(), ToNotFound, ToValidationProblem, ToConflict, ToChallenge);

                ValidateChallenge(results);
            }
        }

        private static void ValidateBadRequest(Results<Ok, NotFound<ProblemDetails>, ValidationProblem, Conflict, ChallengeHttpResult, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
            results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be(ERROR_MESSAGE);
            results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
        }

        private static void ValidateNotFound(Results<Ok, NotFound<ProblemDetails>, ValidationProblem, Conflict, ChallengeHttpResult, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<NotFound<ProblemDetails>>();
            results.Result.As<NotFound<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            results.Result.As<NotFound<ProblemDetails>>().Value.Detail.Should().Be($"Unable to find key : '{KEY}' in source : '{SOURCE_DESCRIPTION}'");
        }

        private static void ValidateValidationProblem(Results<Ok, NotFound<ProblemDetails>, ValidationProblem, Conflict, ChallengeHttpResult, BadRequest<ProblemDetails>> results)
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

        private static void ValidateConflict(Results<Ok, NotFound<ProblemDetails>, ValidationProblem, Conflict, ChallengeHttpResult, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<Conflict>();
        }

        private static void ValidateChallenge(Results<Ok, NotFound<ProblemDetails>, ValidationProblem, Conflict, ChallengeHttpResult, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<ChallengeHttpResult>();
        }

        private static void ValidateOk(Results<Ok, NotFound<ProblemDetails>, ValidationProblem, Conflict, ChallengeHttpResult, BadRequest<ProblemDetails>> results)
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

    private static Option<Conflict> ToConflict(IResultError resultError, HttpContext context)
    {
        if (resultError is ResultErrorConflict)
        {
            var notFound = TypedResults.Conflict();

            return notFound.ToOption();
        }

        return Option.None<Conflict>();
    }

    private static ResultErrorConflict CreateResultErrorConflict() =>
        new();

    public class ResultErrorConflict : IResultErrorNonComposite
    {
        public ResultErrorStructure GetErrorStructureSafe() =>
            throw new NotImplementedException();

        public ResultErrorStructure GetErrorStructure() =>
            throw new NotImplementedException();

        public string GetErrorStringSafe() =>
            throw new NotImplementedException();

        public string GetErrorString() =>
            throw new NotImplementedException();
    }

    private static Option<ChallengeHttpResult> ToChallenge(IResultError resultError, HttpContext context)
    {
        if (resultError is ResultErrorChallenge)
        {
            var notFound = TypedResults.Challenge();

            return notFound.ToOption();
        }

        return Option.None<ChallengeHttpResult>();
    }

    private static ResultErrorChallenge CreateResultErrorChallenge() =>
        new();

    public class ResultErrorChallenge : IResultErrorNonComposite
    {
        public ResultErrorStructure GetErrorStructureSafe() =>
            throw new NotImplementedException();

        public ResultErrorStructure GetErrorStructure() =>
            throw new NotImplementedException();

        public string GetErrorStringSafe() =>
            throw new NotImplementedException();

        public string GetErrorString() =>
            throw new NotImplementedException();
    }
}