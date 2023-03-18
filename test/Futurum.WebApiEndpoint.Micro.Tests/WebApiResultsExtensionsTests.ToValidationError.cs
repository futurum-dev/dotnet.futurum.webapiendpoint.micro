using FluentAssertions;

using Futurum.Core.Result;
using Futurum.Test.Option;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Futurum.WebApiEndpoint.Micro.Tests;

public class WebApiResultsExtensionsToValidationErrorTests
{
    private const string ERROR_MESSAGE = "ErrorMessage";
    private const string REQUEST_PATH = "/RequestPath";

    private const string Property1 = "Property1";
    private const string Property2 = "Property2";

    private const string ErrorMessage1 = "ErrorMessage1";
    private const string ErrorMessage2 = "ErrorMessage2";
    private const string ErrorMessage3 = "ErrorMessage3";


    [Fact]
    public void when_ValidationResultError()
    {
        var validationErrors = new List<ValidationError>
        {
            new(Property1, new[] { ErrorMessage1, ErrorMessage3 }),
            new(Property2, new[] { ErrorMessage2 })
        };

        var resultError = validationErrors.ToResultError();

        var results = ToValidationProblem(resultError, CreateHttpContext());

        results.ShouldBeHasValueWithValueAssertion(x =>
        {
            var errors = new Dictionary<string, string[]>
            {
                { Property1, new[] { ErrorMessage1, ErrorMessage3 } },
                { Property2, new[] { ErrorMessage2 } }
            };

            x.Should().BeOfType<ValidationProblem>();
            x.ProblemDetails.Instance.Should().Be(REQUEST_PATH);
            x.ProblemDetails.As<HttpValidationProblemDetails>().Errors.Should().BeEquivalentTo(errors);
        });
    }

    [Fact]
    public void when_not_ValidationResultError()
    {
        var resultError = ERROR_MESSAGE.ToResultError();

        var results = ToNotFound(resultError, CreateHttpContext());

        results.ShouldBeHasNoValue();
    }

    private static DefaultHttpContext CreateHttpContext() =>
        new() { Request = { Path = REQUEST_PATH } };
}