using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

using FluentAssertions;

using Futurum.WebApiEndpoint.Micro.Sample.Blog;

using Microsoft.AspNetCore.Mvc.Testing;

namespace Futurum.WebApiEndpoint.Micro.EndToEndTests;

[Collection("Sequential")]
public class SamplesEndToEndBlogTests2
{
    [Fact]
    public async Task Get()
    {
        var httpClient = CreateClient();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("/api/v1/blog"),
        };

        var httpResponseMessage = await httpClient.SendAsync(request);

        httpResponseMessage.EnsureSuccessStatusCode();
        var response = await httpResponseMessage.Content.ReadFromJsonAsync<DataCollectionDto<BlogDto>>();

        response.Data.Should().BeEmpty();
    }

    [Fact]
    public async Task Create()
    {
        var httpClient = CreateClient();

        var commandDto = new BlogDto(0, Guid.NewGuid().ToString());
        var json = JsonSerializer.Serialize(commandDto);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("/api/v1/blog"),
            Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json)
        };

        var httpResponseMessage = await httpClient.SendAsync(request);

        httpResponseMessage.EnsureSuccessStatusCode();
        var response = await httpResponseMessage.Content.ReadFromJsonAsync<BlogDto>();

        response.Should().BeEquivalentTo(commandDto);
    }

    [Fact]
    public async Task Composite()
    {
        var httpClient = CreateClient();

        var commandDto = new BlogDto(0, Guid.NewGuid().ToString());
        var json = JsonSerializer.Serialize(commandDto);

        var createRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("/api/v1/blog"),
            Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json)
        };

        var createHttpResponseMessage = await httpClient.SendAsync(createRequest);

        createHttpResponseMessage.EnsureSuccessStatusCode();
        var createResponse = await createHttpResponseMessage.Content.ReadFromJsonAsync<BlogDto>();

        createResponse.Url.Should().Be(commandDto.Url);

        var getRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("/api/v1/blog"),
        };

        var getHttpResponseMessage = await httpClient.SendAsync(getRequest);

        getHttpResponseMessage.EnsureSuccessStatusCode();
        var getResponse = await getHttpResponseMessage.Content.ReadFromJsonAsync<DataCollectionDto<BlogDto>>();

        getResponse.Count.Should().Be(1);
        getResponse.Data.Single().Url.Should().Be(commandDto.Url);
    }

    private static HttpClient CreateClient() =>
        new WebApplicationFactory<Sample.Program>()
            .CreateClient();
}
