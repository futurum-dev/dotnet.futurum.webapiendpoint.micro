using System.Reflection;
using System.Text.Json;

using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;

using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace Futurum.WebApiEndpoint.Micro;

public class FormFilesWithPayload<TPayload> : IBindableFromHttpContext<FormFilesWithPayload<TPayload>>,
                                              IEndpointParameterMetadataProvider
{
    private static readonly JsonSerializerOptions DefaultSerializerOptions = new(JsonSerializerDefaults.Web);

    public required TPayload Payload { get; init; }

    public required IFormFileCollection Files { get; init; } = new FormFileCollection();

    public static async ValueTask<FormFilesWithPayload<TPayload>?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        var jsonOptions = context.RequestServices.GetService<JsonOptions>()?.SerializerOptions ?? DefaultSerializerOptions;

        var form = await context.Request.ReadFormAsync();

        var payloadStringValues = form[nameof(Payload).ToLower()].ToString();
        var payload = JsonSerializer.Deserialize<TPayload>(payloadStringValues, jsonOptions);

        if (payload == null)
        {
            throw new JsonException($"Failed to deserialize {nameof(Payload).ToLower()}: '{payloadStringValues}' as type '{typeof(TPayload).Name}'");
        }

        var files = form.Files;

        return new FormFilesWithPayload<TPayload>
        {
            Payload = payload,
            Files = files
        };
    }

    public static void PopulateMetadata(ParameterInfo parameter, EndpointBuilder builder)
    {
        builder.Metadata.Add(new ConsumesAttribute(typeof(FormFilesWithPayload<TPayload>), "multipart/form-data"));
    }
}