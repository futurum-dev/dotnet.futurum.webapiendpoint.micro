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
    public async Task Exception()
    {
        var httpClient = CreateClient();

        var requestPath = "/v1/error/exception";

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(requestPath),
        };

        var httpResponseMessage = await httpClient.SendAsync(request);

        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        var response = await httpResponseMessage.Content.ReadFromJsonAsync<ProblemDetails>();

        response.Title.Should().Be("An error occurred while processing your request.");
        response.Detail.Should().Be("An error occurred.");
        response.Status.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    private static HttpClient CreateClient() =>
        new WebApplicationFactory<Sample.Program>()
            .CreateClient();
}
