using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

using FluentAssertions;

using Futurum.WebApiEndpoint.Micro.Sample.Features;

using Microsoft.AspNetCore.Mvc.Testing;

namespace Futurum.WebApiEndpoint.Micro.EndToEndTests;

[Collection("Sequential")]
public class FeatureEndToEndFileTests
{
    [Fact]
    public async Task UploadFile()
    {
        var httpClient = CreateClient();

        await using var fileStream = File.OpenRead("./Data/hello-world.txt");

        using var multipartFormDataContent = new MultipartFormDataContent();
        multipartFormDataContent.Add(new StreamContent(fileStream), name: "file", fileName: "hello-world.txt");

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("/v1/file/upload"),
            Content = multipartFormDataContent
        };

        var httpResponseMessage = await httpClient.SendAsync(request);

        httpResponseMessage.EnsureSuccessStatusCode();
        var response = await httpResponseMessage.Content.ReadFromJsonAsync<FileDetailsDto>();

        response.Name.Should().Be("hello-world.txt");
    }

    [Fact]
    public async Task UploadFiles()
    {
        var httpClient = CreateClient();

        await using var fileStream = File.OpenRead("./Data/hello-world.txt");

        using var multipartFormDataContent = new MultipartFormDataContent();
        multipartFormDataContent.Add(new StreamContent(fileStream), name: "files", fileName: "hello-world.txt");

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("/v1/file/uploads"),
            Content = multipartFormDataContent
        };

        var httpResponseMessage = await httpClient.SendAsync(request);

        httpResponseMessage.EnsureSuccessStatusCode();
        var response = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<FileDetailsDto>>();

        response.Count().Should().Be(1);
        response.Single().Name.Should().Be("hello-world.txt");
    }
    
    [Fact]
    public async Task UploadFileWithPayload()
    {
        var httpClient = CreateClient();

        await using var fileStream = File.OpenRead("./Data/hello-world.txt");

        var id = Guid.NewGuid().ToString();
        var payload = new PayloadDto(id);

        using var multipartFormDataContent = new MultipartFormDataContent();
        multipartFormDataContent.Add(new StreamContent(fileStream), name: "file", fileName: "hello-world.txt");
        multipartFormDataContent.Add(new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, MediaTypeNames.Application.Json), name: "payload");

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("/v1/file/upload-with-payload"),
            Content = multipartFormDataContent
        };

        var httpResponseMessage = await httpClient.SendAsync(request);

        httpResponseMessage.EnsureSuccessStatusCode();
        var response = await httpResponseMessage.Content.ReadFromJsonAsync<FileDetailsWithPayloadDto>();

        response.Name.Should().Be("hello-world.txt");
        response.Payload.Should().Be(id);
    }
    
    [Fact]
    public async Task UploadFilesWithPayload()
    {
        var httpClient = CreateClient();

        await using var fileStream = File.OpenRead("./Data/hello-world.txt");

        var id = Guid.NewGuid().ToString();
        var payload = new PayloadDto(id);

        using var multipartFormDataContent = new MultipartFormDataContent();
        multipartFormDataContent.Add(new StreamContent(fileStream), name: "files", fileName: "hello-world.txt");
        multipartFormDataContent.Add(new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, MediaTypeNames.Application.Json), name: "payload");

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("/v1/file/uploads-with-payload"),
            Content = multipartFormDataContent
        };

        var httpResponseMessage = await httpClient.SendAsync(request);

        httpResponseMessage.EnsureSuccessStatusCode();
        var response = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<FileDetailsWithPayloadDto>>();

        response.Count().Should().Be(1);
        response.Single().Name.Should().Be("hello-world.txt");
        response.Single().Payload.Should().Be(id);
    }

    [Fact]
    public async Task Download()
    {
        var httpClient = CreateClient();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("/v1/file/download"),
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