using System.Reflection;
using System.Text.Json;

using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;

using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace Futurum.WebApiEndpoint.Micro;

/// <summary>
/// Represents a file and payload sent with the HttpRequest.
/// </summary>
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

        var payload = GetPayload(form, jsonOptions);

        var file = GetFile(form);

        return payload != null && file != null
            ? new FormFileWithPayload<TPayload>
            {
                Payload = payload,
                File = file
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

    private static IFormFile? GetFile(IFormCollection form) =>
        !form.Files.Any()
            ? default
            : form.Files.Single();

    public static void PopulateMetadata(ParameterInfo parameter, EndpointBuilder builder)
    {
        builder.Metadata.Add(new ConsumesAttribute(typeof(FormFileWithPayload<TPayload>), "multipart/form-data"));
    }
}
