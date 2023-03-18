using FluentAssertions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Futurum.WebApiEndpoint.Micro.Tests;

public class WebApiResultsExtensionsToCreatedTests
{
    private const string VALUE = "Value";
    private const string REQUEST_PATH = "/created";
    private const string URI = "http://uri.com/";

    public class without_value
    {
        [Fact]
        public void without_Uri()
        {
            var value = VALUE;

            var results = ToCreated(CreateHttpContextWithRequestPath());

            results.Should().BeOfType<Created>();
            results.Location.Should().Be(REQUEST_PATH);
        }

        [Fact]
        public void with_Uri_string()
        {
            var results = ToCreated(URI)(CreateHttpContextWithRequestPath());

            results.Should().BeOfType<Created>();
            results.Location.Should().Be(URI);
        }

        [Fact]
        public void with_Uri()
        {
            var results = ToCreated(new Uri(URI))(CreateHttpContextWithRequestPath());

            results.Should().BeOfType<Created>();
            results.Location.Should().Be(URI);
        }
    }

    public class with_value
    {
        [Fact]
        public void without_Uri()
        {
            var value = VALUE;

            var results = ToCreated(CreateHttpContextWithRequestPath(), value);

            results.Should().BeOfType<Created<string>>();
            results.Value.Should().Be(VALUE);
            results.Location.Should().Be(REQUEST_PATH);
        }

        [Fact]
        public void with_Uri_string()
        {
            var value = VALUE;

            var results = ToCreated<string>(URI)(CreateHttpContextWithRequestPath(), value);

            results.Should().BeOfType<Created<string>>();
            results.Value.Should().Be(VALUE);
            results.Location.Should().Be(URI);
        }

        [Fact]
        public void with_Uri()
        {
            var value = VALUE;

            var results = ToCreated<string>(new Uri(URI))(CreateHttpContextWithRequestPath(), value);

            results.Should().BeOfType<Created<string>>();
            results.Value.Should().Be(VALUE);
            results.Location.Should().Be(URI);
        }
    }

    private static DefaultHttpContext CreateHttpContextWithRequestPath() =>
        new() { Request = { Path = REQUEST_PATH } };
}