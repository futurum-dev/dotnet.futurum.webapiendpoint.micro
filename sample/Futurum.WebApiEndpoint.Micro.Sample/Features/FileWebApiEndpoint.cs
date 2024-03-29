using System.Net.Mime;
using System.Text.Json.Serialization;

namespace Futurum.WebApiEndpoint.Micro.Sample.Features;

[WebApiEndpoint(prefixRoute: "file", group: "feature")]
public partial class FileWebApiEndpoint
{
    protected override void Build(IEndpointRouteBuilder builder)
    {
        builder.MapPost("upload", UploadHandler)
               .DisableAntiforgery();

        builder.MapPost("uploads", UploadsHandler)
               .DisableAntiforgery();

        builder.MapPost("upload-with-payload", UploadWithPayloadHandler);

        builder.MapPost("uploads-with-payload", UploadsWithPayloadHandler);

        builder.MapGet("download", DownloadHandler);
    }

    private static Task<Results<Ok<FileDetailsDto>, BadRequest<ProblemDetails>>> UploadHandler(HttpContext context, IFormFile file)
    {
        return RunToOkAsync(Execute, context, "Failed to read file");

        async Task<FileDetailsDto> Execute()
        {
            var tempFile = Path.GetTempFileName();
            await using var stream = File.OpenWrite(tempFile);
            await file.CopyToAsync(stream);

            return new FileDetailsDto(file.FileName);
        }
    }

    private static Task<Results<Ok<IEnumerable<FileDetailsDto>>, BadRequest<ProblemDetails>>> UploadsHandler(HttpContext context, IFormFileCollection files)
    {
        return RunToOkAsync(Execute, context, "Failed to read file");

        async Task<IEnumerable<FileDetailsDto>> Execute()
        {
            var fileDetails = new List<FileDetailsDto>();

            foreach (var file in files)
            {
                var tempFile = Path.GetTempFileName();
                await using var stream = File.OpenWrite(tempFile);
                await file.CopyToAsync(stream);

                fileDetails.Add(new FileDetailsDto(file.FileName));
            }

            return fileDetails;
        }
    }

    private static Task<Results<Ok<FileDetailsWithPayloadDto>, BadRequest<ProblemDetails>>> UploadWithPayloadHandler(HttpContext context, FormFileWithPayload<PayloadDto> fileWithPayload)
    {
        return RunToOkAsync(Execute, context, "Failed to read file");

        async Task<FileDetailsWithPayloadDto> Execute()
        {
            var tempFile = Path.GetTempFileName();
            await using var stream = File.OpenWrite(tempFile);
            await fileWithPayload.File.CopyToAsync(stream);

            return new FileDetailsWithPayloadDto(fileWithPayload.File.FileName, fileWithPayload.Payload.Name);
        }
    }

    private static Task<Results<Ok<IEnumerable<FileDetailsWithPayloadDto>>, BadRequest<ProblemDetails>>> UploadsWithPayloadHandler(
        HttpContext context, FormFilesWithPayload<PayloadDto> filesWithPayload)
    {
        return RunToOkAsync(Execute, context, "Failed to read file");

        async Task<IEnumerable<FileDetailsWithPayloadDto>> Execute()
        {
            var fileDetails = new List<FileDetailsWithPayloadDto>();

            foreach (var file in filesWithPayload.Files)
            {
                var tempFile = Path.GetTempFileName();
                await using var stream = File.OpenWrite(tempFile);
                await file.CopyToAsync(stream);

                fileDetails.Add(new FileDetailsWithPayloadDto(file.FileName, filesWithPayload.Payload.Name));
            }

            return fileDetails;
        }
    }

    private static Results<NotFound, FileStreamHttpResult, BadRequest<ProblemDetails>> DownloadHandler(HttpContext context)
    {
        return Run(Execute, context, "Failed to read file");

        Results<NotFound, FileStreamHttpResult> Execute()
        {
            var path = "./Data/hello-world.txt";

            if (!File.Exists(path))
            {
                return TypedResults.NotFound();
            }

            var fileStream = File.OpenRead(path);
            return TypedResults.File(fileStream, MediaTypeNames.Application.Octet, "hello-world.txt");
        }
    }
}

public record FileDetailsDto(string Name);

public record PayloadDto([property: JsonPropertyName("name")] string Name);

public record FileDetailsWithPayloadDto(string Name, string Payload);
