using System.Text.Json;

using FluentAssertions;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace Futurum.WebApiEndpoint.Micro.Tests;

public class FormFileWithPayloadTests
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
        httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues> { { "payload", payloadJson } }, new FormFileCollection { formFile });

        var services = new ServiceCollection();
        httpContext.RequestServices = services.BuildServiceProvider();

        var result = await FormFileWithPayload<PayloadDto>.BindAsync(httpContext, null);

        result.Should().NotBeNull();

        result.File.Name.Should().Be("hello-world.txt");
        result.File.FileName.Should().Be("hello-world.txt");
        result.File.Length.Should().Be(fileStream.Length);

        result.Payload.Name.Should().Be(id);
    }

    public record PayloadDto(string Name);
}