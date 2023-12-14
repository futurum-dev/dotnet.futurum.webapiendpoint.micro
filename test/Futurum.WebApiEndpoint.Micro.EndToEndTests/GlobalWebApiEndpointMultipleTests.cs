using System.Net;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc.Testing;

namespace Futurum.WebApiEndpoint.Micro.EndToEndTests;

[Collection("Sequential")]
public class GlobalWebApiEndpointTests
{
    [Fact]
    public async Task check_v1_works()
    {
        var httpClient = CreateClient();

        var name = Guid.NewGuid().ToString();

        var httpResponseMessage = await httpClient.GetAsync($"/api/v1/greeting/hello?name={name}");

        httpResponseMessage.EnsureSuccessStatusCode();
        var response = await httpResponseMessage.Content.ReadAsStringAsync();

        response.Should().Be($"\"Hello {name}\"");
    }

    [Fact]
    public async Task check_v2_does_not_works()
    {
        var httpClient = CreateClient();

        var name = Guid.NewGuid().ToString();

        var httpResponseMessage = await httpClient.GetAsync($"/api-2/v1/greeting/hello?name={name}");

        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private static HttpClient CreateClient() =>
        new WebApplicationFactory<Sample.Program>()
            .CreateClient();
}
