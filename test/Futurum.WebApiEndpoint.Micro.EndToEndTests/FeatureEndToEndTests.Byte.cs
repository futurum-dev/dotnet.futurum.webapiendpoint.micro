using System.Net.Mime;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc.Testing;

namespace Futurum.WebApiEndpoint.Micro.EndToEndTests;

[Collection("Sequential")]
public class FeatureEndToEndByteTests
{
    [Fact]
    public async Task Bytes()
    {
        var httpClient = CreateClient();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("/api/v1/bytes/download"),
        };

        var httpResponseMessage = await httpClient.SendAsync(request);

        httpResponseMessage.EnsureSuccessStatusCode();
        var bytes = await httpResponseMessage.Content.ReadAsByteArrayAsync();

        await using var expectedFileStream = File.OpenRead("./Data/hello-world.txt");

        var expectedBytes = new byte[expectedFileStream.Length];
        await expectedFileStream.ReadAsync(expectedBytes, CancellationToken.None);

        bytes.Should().BeEquivalentTo(expectedBytes);
        httpResponseMessage.Content.Headers.GetValues("Content-Disposition").Single().Should().Be("attachment; filename=hello-world.txt; filename*=UTF-8''hello-world.txt");
        httpResponseMessage.Content.Headers.GetValues("Content-Type").Single().Should().Be(MediaTypeNames.Application.Octet);
        httpResponseMessage.Content.Headers.GetValues("Content-Length").Single().Should().Be(expectedFileStream.Length.ToString());
    }

    private static HttpClient CreateClient() =>
        new WebApplicationFactory<Sample.Program>()
            .CreateClient();
}
