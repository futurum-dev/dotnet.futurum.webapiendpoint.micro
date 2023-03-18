using System.Net;

using FluentAssertions;

using Futurum.Core.Result;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.Micro.Tests;

public class WebApiResultsExtensionsToWebApiBadRequestOnlyTests
{
    private const string ERROR_MESSAGE = "ErrorMessage";
    private const string REQUEST_PATH = "/RequestPath";

    private const string VALUE = "Value";

    public class NonGeneric
    {
        public class Sync
        {
            [Fact]
            public void result_success()
            {
                var result = Result.Ok();

                var results = result.ToWebApi(CreateHttpContext(), ToOk);

                ValidateOk(results);
            }

            [Fact]
            public void result_failure()
            {
                var results = Result.Fail(ERROR_MESSAGE).ToWebApi(CreateHttpContext(), ToOk);

                ValidateBadRequest(results);
            }
        }
        public class Async
        {
            [Fact]
            public async Task result_success()
            {
                var result = Result.OkAsync();

                var results = await result.ToWebApiAsync(CreateHttpContext(), ToOk);

                ValidateOk(results);
            }

            [Fact]
            public async Task result_failure()
            {
                var results = await Result.FailAsync(ERROR_MESSAGE).ToWebApiAsync(CreateHttpContext(), ToOk);

                ValidateBadRequest(results);
            }
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

                var results = result.ToWebApi(CreateHttpContext(), ToOk);

                ValidateOk(results);
            }

            [Fact]
            public void result_failure()
            {
                var results = Result.Fail<string>(ERROR_MESSAGE).ToWebApi(CreateHttpContext(), ToOk);

                ValidateBadRequest(results);
            }
        }
        public class Async
        {
            [Fact]
            public async Task result_success()
            {
                var result = Result.OkAsync(VALUE);

                var results = await result.ToWebApiAsync(CreateHttpContext(), ToOk);

                ValidateOk(results);
            }

            [Fact]
            public async Task result_failure()
            {
                var results = await Result.FailAsync<string>(ERROR_MESSAGE).ToWebApiAsync(CreateHttpContext(), ToOk);

                ValidateBadRequest(results);
            }
        }
    }

    private static void ValidateBadRequest(Results<Ok, BadRequest<ProblemDetails>> results)
    {
        results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
        results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be(ERROR_MESSAGE);
        results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
    }

    private static void ValidateBadRequest(Results<Ok<string>, BadRequest<ProblemDetails>> results)
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

    private static void ValidateOk<T>(Results<Ok<T>, BadRequest<ProblemDetails>> results)
    {
        results.Result.Should().BeOfType<Ok<T>>();
        results.Result.As<Ok<T>>().Value.Should().Be(VALUE);
    }

    private static DefaultHttpContext CreateHttpContext() =>
        new() { Request = { Path = REQUEST_PATH } };
}