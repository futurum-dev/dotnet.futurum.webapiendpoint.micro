using System.Text.Json;

using FluentAssertions;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace Futurum.WebApiEndpoint.Micro.Tests;

public class FormFilesWithPayloadTests
{
    [Fact]
    public async Task check()
    {
        await using var fileStream = File.OpenRead("./Data/hello-world.txt");

        var formFile = new FormFile(fileStream, 0, fileStream.Length, "hello-world.txt", "hello-world.txt");

        var id = Guid.NewGuid().ToString();
        var payload = new PayloadDto(id);

        var payloadJson = JsonSerializer.Serialize(payload, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues> { { "payload", payloadJson } }, new FormFileCollection { formFile, formFile });

        var services = new ServiceCollection();
        httpContext.RequestServices = services.BuildServiceProvider();

        var result = await FormFilesWithPayload<PayloadDto>.BindAsync(httpContext, null);

        result.Should().NotBeNull();

        result.Files.Count.Should().Be(2);

        result.Files.First().Name.Should().Be("hello-world.txt");
        result.Files.First().FileName.Should().Be("hello-world.txt");
        result.Files.First().Length.Should().Be(fileStream.Length);

        result.Files.Skip(1).First().Name.Should().Be("hello-world.txt");
        result.Files.Skip(1).First().FileName.Should().Be("hello-world.txt");
        result.Files.Skip(1).First().Length.Should().Be(fileStream.Length);

        result.Payload.Name.Should().Be(id);
    }

    [Fact]
    public async Task when_Payload_missing_return_null()
    {
        await using var fileStream = File.OpenRead("./Data/hello-world.txt");

        var formFile = new FormFile(fileStream, 0, fileStream.Length, "hello-world.txt", "hello-world.txt");

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection { formFile, formFile });

        var services = new ServiceCollection();
        httpContext.RequestServices = services.BuildServiceProvider();

        var result = await FormFilesWithPayload<PayloadDto>.BindAsync(httpContext, null);

        result.Should().BeNull();
    }

    [Fact]
    public async Task when_File_missing_return_null()
    {
        var id = Guid.NewGuid().ToString();
        var payload = new PayloadDto(id);

        var payloadJson = JsonSerializer.Serialize(payload, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues> { { "payload", payloadJson } }, new FormFileCollection());

        var services = new ServiceCollection();
        httpContext.RequestServices = services.BuildServiceProvider();

        var result = await FormFilesWithPayload<PayloadDto>.BindAsync(httpContext, null);

        result.Should().BeNull();
    }

    public record PayloadDto(string Name);
}