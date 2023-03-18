using System.Reflection;
using System.Text.Json;

using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;

using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace Futurum.WebApiEndpoint.Micro;

public class FormFileWithPayload<TPayload> : IBindableFromHttpContext<FormFileWithPayload<TPayload>>,
                                             IEndpointParameterMetadataProvider
{
    private static readonly JsonSerializerOptions DefaultSerializerOptions = new(JsonSerializerDefaults.Web);

    public required TPayload Payload { get; init; }

    public required IFormFile File { get; init; }

    public static async ValueTask<FormFileWithPayload<TPayload>?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        var jsonOptions = context.RequestServices.GetService<JsonOptions>()?.SerializerOptions ?? DefaultSerializerOptions;

        var form = await context.Request.ReadFormAsync();

        var payloadStringValues = form[nameof(Payload).ToLower()].ToString();
        var payload = JsonSerializer.Deserialize<TPayload>(payloadStringValues, jsonOptions);

        if (payload == null)
        {
            throw new JsonException($"Failed to deserialize {nameof(Payload).ToLower()}: '{payloadStringValues}' as type '{typeof(TPayload).Name}'");
        }

        if (!form.Files.Any())
        {
            throw new InvalidOperationException("No file found in request");
        }

        var file = form.Files.Single();

        return new FormFileWithPayload<TPayload>
        {
            Payload = payload,
            File = file
        };
    }

    public static void PopulateMetadata(ParameterInfo parameter, EndpointBuilder builder)
    {
        builder.Metadata.Add(new ConsumesAttribute(typeof(FormFileWithPayload<TPayload>), "multipart/form-data"));
    }
}