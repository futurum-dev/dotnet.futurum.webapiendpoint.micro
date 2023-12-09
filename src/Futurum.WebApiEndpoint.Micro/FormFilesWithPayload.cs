using System.Globalization;
using System.Reflection;
using System.Text.Json;

using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;

using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace Futurum.WebApiEndpoint.Micro;

/// <summary>
/// Represents the collection of files and payload sent with the HttpRequest.
/// </summary>
public class FormFilesWithPayload<TPayload> : IBindableFromHttpContext<FormFilesWithPayload<TPayload>>,
                                              IEndpointParameterMetadataProvider
{
    private static readonly JsonSerializerOptions DefaultSerializerOptions = new(JsonSerializerDefaults.Web);

    public required TPayload Payload { get; init; }

    public required IFormFileCollection Files { get; init; } = new FormFileCollection();

#pragma warning disable CA1000
    public static async ValueTask<FormFilesWithPayload<TPayload>?> BindAsync(HttpContext context, ParameterInfo parameter)
#pragma warning restore CA1000
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
        if (form.TryGetValue(nameof(Payload).ToLower(CultureInfo.CurrentCulture), out var payloadStringValues))
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

#pragma warning disable CA1000
    public static void PopulateMetadata(ParameterInfo parameter, EndpointBuilder builder)
#pragma warning restore CA1000
    {
        builder.Metadata.Add(new ConsumesAttribute(typeof(FormFilesWithPayload<TPayload>), "multipart/form-data"));
    }
}
