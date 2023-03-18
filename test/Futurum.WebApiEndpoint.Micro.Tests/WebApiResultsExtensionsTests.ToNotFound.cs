using System.Net;

using FluentAssertions;

using Futurum.Core.Result;
using Futurum.Test.Option;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Futurum.WebApiEndpoint.Micro.Tests;

public class WebApiResultsExtensionsToNotFoundTests
{
    private const string ERROR_MESSAGE = "ErrorMessage";
    private const string REQUEST_PATH = "/RequestPath";

    private const string KEY = "KEY";
    private const string SOURCE_DESCRIPTION = "Source Description";

    [Fact]
    public void when_ResultErrorKeyNotFound()
    {
        var resultError = ResultErrorKeyNotFound.Create(KEY, SOURCE_DESCRIPTION);

        var results = ToNotFound(resultError, CreateHttpContext());

        results.ShouldBeHasValueWithValueAssertion(x =>
        {
            x.Should().BeOfType<NotFound<ProblemDetails>>();
            x.Value.Should().NotBeNull();
            x.Value.Detail.Should().Be($"Unable to find key : '{KEY}' in source : '{SOURCE_DESCRIPTION}'");
            x.Value.Instance.Should().Be(REQUEST_PATH);
            x.Value.Status.Should().Be((int)HttpStatusCode.NotFound);
            x.Value.Title.Should().Be(ReasonPhrases.GetReasonPhrase((int)HttpStatusCode.NotFound));
        });
    }

    [Fact]
    public void when_not_ResultErrorKeyNotFound()
    {
        var resultError = ERROR_MESSAGE.ToResultError();

        var results = ToNotFound(resultError, CreateHttpContext());

        results.ShouldBeHasNoValue();
    }

    private static DefaultHttpContext CreateHttpContext() =>
        new() { Request = { Path = REQUEST_PATH } };
}