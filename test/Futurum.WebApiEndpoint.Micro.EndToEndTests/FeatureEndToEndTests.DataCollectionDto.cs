using System.Net.Http.Json;

using FluentAssertions;

using Futurum.WebApiEndpoint.Micro.Sample.Features;

using Microsoft.AspNetCore.Mvc.Testing;

namespace Futurum.WebApiEndpoint.Micro.EndToEndTests;

[Collection("Sequential")]
public class FeatureEndToEndDataCollectionDtoTests
{
    [Fact]
    public async Task DataCollection()
    {
        var httpClient = CreateClient();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("/v1/data-collection"),
        };

        var httpResponseMessage = await httpClient.SendAsync(request);

        httpResponseMessage.EnsureSuccessStatusCode();
        var response = await httpResponseMessage.Content.ReadFromJsonAsync<DataCollectionDto<FeatureDto>>();

        response.Count.Should().Be(10);
        response.Data.Should().BeEquivalentTo(Enumerable.Range(0, 10).Select(i => new FeatureDto($"Name - {i}")));
    }

    private static HttpClient CreateClient() =>
        new WebApplicationFactory<Sample.Program>()
            .CreateClient();
}