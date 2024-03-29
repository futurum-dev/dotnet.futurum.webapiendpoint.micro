using System.Net.Mime;

namespace Futurum.WebApiEndpoint.Micro.Sample.Features;

[WebApiEndpoint("bytes", "feature")]
public partial class BytesWebApiEndpoint
{
    protected override void Build(IEndpointRouteBuilder builder)
    {
        builder.MapGet("download", DownloadHandler);
    }

    private static Results<NotFound, FileContentHttpResult, BadRequest<ProblemDetails>> DownloadHandler(HttpContext context)
    {
        return Run(Execute, context, "Failed to read file");

        Results<NotFound, FileContentHttpResult> Execute()
        {
            var path = "./Data/hello-world.txt";

            if (!File.Exists(path))
            {
                return TypedResults.NotFound();
            }

            var bytes = File.ReadAllBytes(path);
            return TypedResults.Bytes(bytes, MediaTypeNames.Application.Octet, "hello-world.txt");
        }
    }

    private static Task<Results<NotFound, FileContentHttpResult, BadRequest<ProblemDetails>>> DownloadHandlerAsync(HttpContext context)
    {
        return RunAsync(Execute, context, "Failed to read file");

        async Task<Results<NotFound, FileContentHttpResult>> Execute()
        {
            var path = "./Data/hello-world.txt";

            if (!File.Exists(path))
            {
                return TypedResults.NotFound();
            }

            var bytes = await File.ReadAllBytesAsync(path);
            return TypedResults.Bytes(bytes, MediaTypeNames.Application.Octet, "hello-world.txt");
        }
    }
}
