using System.Net;

using FluentAssertions;

using Futurum.Core.Result;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.Micro.Tests;

public class WebApiResultsExtensionsToWebApiBadRequestErrorIResult1Tests
{
    private const string ERROR_MESSAGE = "ErrorMessage";
    private const string REQUEST_PATH = "/RequestPath";

    private const string VALUE = "Value";

    private const string KEY = "KEY";
    private const string SOURCE_DESCRIPTION = "Source Description";

    public class NonGeneric
    {
        public class Sync
        {
            [Fact]
            public void result_success()
            {
                var result = Result.Ok();

                var results = result.ToWebApi(CreateHttpContext(),
                                              ToOk,
                                              ToNotFound);

                results.Result.Should().BeOfType<Ok>();
            }

            [Fact]
            public void result_failure()
            {
                var results = Result.Fail(ERROR_MESSAGE).ToWebApi(CreateHttpContext(),
                                                                  ToOk,
                                                                  ToNotFound);

                ValidateBadRequest(results);
            }

            [Fact]
            public void result_1_failure()
            {
                var resultError = ResultErrorKeyNotFound.Create(KEY, SOURCE_DESCRIPTION);

                var results = Result.Fail(resultError).ToWebApi(CreateHttpContext(),
                                                                ToOk,
                                                                ToNotFound);

                ValidateNotFound(results);
            }
        }

        public class Async
        {
            [Fact]
            public async Task result_success()
            {
                var result = Result.OkAsync();

                var results = await result.ToWebApiAsync(CreateHttpContext(),
                                                         ToOk,
                                                         ToNotFound);

                results.Result.Should().BeOfType<Ok>();
            }

            [Fact]
            public async Task result_failure()
            {
                var results = await Result.FailAsync(ERROR_MESSAGE).ToWebApiAsync(CreateHttpContext(),
                                                                                  ToOk,
                                                                                  ToNotFound);

                ValidateBadRequest(results);
            }

            [Fact]
            public async Task result_1_failure()
            {
                var resultError = ResultErrorKeyNotFound.Create(KEY, SOURCE_DESCRIPTION);

                var results = await Result.FailAsync(resultError).ToWebApiAsync(CreateHttpContext(),
                                                                                ToOk,
                                                                                ToNotFound);

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
    }

    public class Generic
    {
        public class Sync
        {
            [Fact]
            public void result_success()
            {
                var result = Result.Ok(VALUE);

                var results = result.ToWebApi(CreateHttpContext(),
                                              ToOk,
                                              ToNotFound);

                results.Result.Should().BeOfType<Ok<string>>();
                results.Result.As<Ok<string>>().Value.Should().Be(VALUE);
            }

            [Fact]
            public void result_failure()
            {
                var results = Result.Fail<string>(ERROR_MESSAGE).ToWebApi(CreateHttpContext(),
                                                                          ToOk,
                                                                          ToNotFound);

                ValidateBadRequest(results);
            }

            [Fact]
            public void result_1_failure()
            {
                var resultError = ResultErrorKeyNotFound.Create(KEY, SOURCE_DESCRIPTION);

                var results = Result.Fail<string>(resultError).ToWebApi(CreateHttpContext(),
                                                                        ToOk,
                                                                        ToNotFound);

                ValidateNotFound(results);
            }
        }

        public class Async
        {
            [Fact]
            public async Task result_success()
            {
                var result = Result.OkAsync(VALUE);

                var results = await result.ToWebApiAsync(CreateHttpContext(),
                                                         ToOk,
                                                         ToNotFound);

                results.Result.Should().BeOfType<Ok<string>>();
                results.Result.As<Ok<string>>().Value.Should().Be(VALUE);
            }

            [Fact]
            public async Task result_failure()
            {
                var results = await Result.FailAsync<string>(ERROR_MESSAGE).ToWebApiAsync(CreateHttpContext(),
                                                                                          ToOk,
                                                                                          ToNotFound);

                ValidateBadRequest(results);
            }

            [Fact]
            public async Task result_1_failure()
            {
                var resultError = ResultErrorKeyNotFound.Create(KEY, SOURCE_DESCRIPTION);

                var results = await Result.FailAsync<string>(resultError).ToWebApiAsync(CreateHttpContext(),
                                                                                        ToOk,
                                                                                        ToNotFound);

                ValidateNotFound(results);
            }
        }

        private static void ValidateBadRequest(Results<Ok<string>, NotFound<ProblemDetails>, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
            results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be(ERROR_MESSAGE);
            results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
        }

        private static void ValidateNotFound(Results<Ok<string>, NotFound<ProblemDetails>, BadRequest<ProblemDetails>> results)
        {
            results.Result.Should().BeOfType<NotFound<ProblemDetails>>();
            results.Result.As<NotFound<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            results.Result.As<NotFound<ProblemDetails>>().Value.Detail.Should().Be($"Unable to find key : '{KEY}' in source : '{SOURCE_DESCRIPTION}'");
        }
    }

    private static DefaultHttpContext CreateHttpContext() =>
        new() { Request = { Path = REQUEST_PATH } };
}