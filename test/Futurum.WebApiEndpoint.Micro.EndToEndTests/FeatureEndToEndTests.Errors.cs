using System.Net;
using System.Net.Http.Json;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.WebUtilities;

namespace Futurum.WebApiEndpoint.Micro.EndToEndTests;

[Collection("Sequential")]
public class FeatureEndToEndErrorsTests
{
    [Fact]
    public async Task ErrorResult()
    {
        var httpClient = CreateClient();

        var requestPath = "/v1/error/result-error";
        
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(requestPath),
        };

        var httpResponseMessage = await httpClient.SendAsync(request);

        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var response = await httpResponseMessage.Content.ReadFromJsonAsync<ProblemDetails>();

        response.Title.Should().Be(ReasonPhrases.GetReasonPhrase((int)HttpStatusCode.BadRequest));
        response.Detail.Should().Be("We have a ResultError!");
        response.Status.Should().Be((int)HttpStatusCode.BadRequest);
        response.Instance.Should().Be(requestPath);
    }
    
    [Fact]
    public async Task ErrorResultWithException()
    {
        var httpClient = CreateClient();

        var requestPath = "/v1/error/exception-with-result-error";
        
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(requestPath),
        };

        var httpResponseMessage = await httpClient.SendAsync(request);

        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var response = await httpResponseMessage.Content.ReadFromJsonAsync<ProblemDetails>();

        response.Title.Should().Be(ReasonPhrases.GetReasonPhrase((int)HttpStatusCode.BadRequest));
        response.Detail.Should().Contain("Exception to ResultError");
        response.Status.Should().Be((int)HttpStatusCode.BadRequest);
        response.Instance.Should().Be(requestPath);
    }

    private static HttpClient CreateClient() =>
        new WebApplicationFactory<Sample.Program>()
            .CreateClient();
}