using FluentAssertions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Futurum.WebApiEndpoint.Micro.Tests;

public class WebApiResultsExtensionsToAcceptedTests
{
    private const string VALUE = "Value";
    private const string REQUEST_PATH = "/Accepted";
    private const string URI = "http://uri.com/";

    public class without_value
    {
        [Fact]
        public void without_Uri()
        {
            var value = VALUE;

            var results = ToAccepted(CreateHttpContextWithRequestPath());

            results.Should().BeOfType<Accepted>();
            results.Location.Should().Be(REQUEST_PATH);
        }

        [Fact]
        public void with_Uri_string()
        {
            var results = ToAccepted(URI)(CreateHttpContextWithRequestPath());

            results.Should().BeOfType<Accepted>();
            results.Location.Should().Be(URI);
        }

        [Fact]
        public void with_Uri()
        {
            var results = ToAccepted(new Uri(URI))(CreateHttpContextWithRequestPath());

            results.Should().BeOfType<Accepted>();
            results.Location.Should().Be(URI);
        }
    }

    public class with_value
    {
        [Fact]
        public void without_Uri()
        {
            var value = VALUE;

            var results = ToAccepted(CreateHttpContextWithRequestPath(), value);

            results.Should().BeOfType<Accepted<string>>();
            results.Value.Should().Be(VALUE);
            results.Location.Should().Be(REQUEST_PATH);
        }

        [Fact]
        public void with_Uri_string()
        {
            var value = VALUE;

            var results = ToAccepted<string>(URI)(CreateHttpContextWithRequestPath(), value);

            results.Should().BeOfType<Accepted<string>>();
            results.Value.Should().Be(VALUE);
            results.Location.Should().Be(URI);
        }

        [Fact]
        public void with_Uri()
        {
            var value = VALUE;

            var results = ToAccepted<string>(new Uri(URI))(CreateHttpContextWithRequestPath(), value);

            results.Should().BeOfType<Accepted<string>>();
            results.Value.Should().Be(VALUE);
            results.Location.Should().Be(URI);
        }
    }

    private static DefaultHttpContext CreateHttpContextWithRequestPath() =>
        new() { Request = { Path = REQUEST_PATH } };
}