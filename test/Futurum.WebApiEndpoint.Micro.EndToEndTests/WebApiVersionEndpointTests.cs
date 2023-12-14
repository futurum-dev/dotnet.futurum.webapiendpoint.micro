using System.Net;
using System.Net.Http.Json;

using FluentAssertions;

using Futurum.WebApiEndpoint.Micro.Sample.Features;

using Microsoft.AspNetCore.Mvc.Testing;

namespace Futurum.WebApiEndpoint.Micro.EndToEndTests;

[Collection("Sequential")]
public class WebApiVersionEndpointTests
{
    [Fact]
    public async Task check_v1_works()
    {
        var httpClient = CreateClient();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("/api/v3/test-api1/openapi"),
        };

        var httpResponseMessage = await httpClient.SendAsync(request);

        httpResponseMessage.EnsureSuccessStatusCode();
        var response = await httpResponseMessage.Content.ReadFromJsonAsync<DataCollectionDto<FeatureDto>>();

        response.Count.Should().Be(10);
        response.Data.Should().BeEquivalentTo(Enumerable.Range(0, 10).Select(i => new FeatureDto($"V1 - Name - {i}")));
    }

    [Fact]
    public async Task check_v2_does_not_works()
    {
        var httpClient = CreateClient();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("/api/v3/test-api2/openapi"),
        };

        var httpResponseMessage = await httpClient.SendAsync(request);

        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private static HttpClient CreateClient() =>
        new WebApplicationFactory<Sample.Program>()
            .CreateClient();
}
