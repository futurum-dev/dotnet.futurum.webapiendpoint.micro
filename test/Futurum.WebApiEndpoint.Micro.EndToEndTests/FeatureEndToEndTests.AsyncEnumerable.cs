using System.Net.Http.Json;

using FluentAssertions;

using Futurum.WebApiEndpoint.Micro.Sample.Features;

using Microsoft.AspNetCore.Mvc.Testing;

namespace Futurum.WebApiEndpoint.Micro.EndToEndTests;

[Collection("Sequential")]
public class FeatureEndToEndAsyncEnumerableTests
{
    [Fact]
    public async Task AsyncEnumerable()
    {
        var httpClient = CreateClient();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("/api/v1/async-enumerable"),
        };

        var httpResponseMessage = await httpClient.SendAsync(request);

        httpResponseMessage.EnsureSuccessStatusCode();
        var response = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<FeatureDto>>();

        response.Count().Should().Be(10);
        response.Should().BeEquivalentTo(Enumerable.Range(0, 10).Select(i => new FeatureDto($"Name - {i}")));
    }

    private static HttpClient CreateClient() =>
        new WebApplicationFactory<Sample.Program>()
            .CreateClient();
}
