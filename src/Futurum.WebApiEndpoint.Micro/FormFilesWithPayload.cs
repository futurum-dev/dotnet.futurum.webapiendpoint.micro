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

        var payload = GetPayload(form, jsonOptions);

        var files = GetFiles(form);

        return payload != null && files != null
            ? new FormFilesWithPayload<TPayload>
            {
                Payload = payload,
                Files = files
            }
            : null;
    }

    private static TPayload? GetPayload(IFormCollection form, JsonSerializerOptions jsonOptions)
    {
        if (form.TryGetValue(nameof(Payload).ToLower(), out var payloadStringValues))
        {
            var payloadString = payloadStringValues.ToString();
            var payload = JsonSerializer.Deserialize<TPayload>(payloadString, jsonOptions);

            return payload;
        }

        return default;
    }

    private static IFormFileCollection? GetFiles(IFormCollection form) =>
        !form.Files.Any()
            ? null
            : form.Files;

    public static void PopulateMetadata(ParameterInfo parameter, EndpointBuilder builder)
    {
        builder.Metadata.Add(new ConsumesAttribute(typeof(FormFilesWithPayload<TPayload>), "multipart/form-data"));
    }
}