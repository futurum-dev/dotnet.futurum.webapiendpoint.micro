namespace Futurum.WebApiEndpoint.Micro.Sample.Blog;

[WebApiEndpoint("blog")]
public partial class BlogWebApiEndpoint
{
    protected override void Build(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/", GetHandler);

        builder.MapGet("/{id}", GetByIdHandler);

        builder.MapPost("/", CreateHandler);

        builder.MapDelete("/{id}", DeleteHandler);

        builder.MapPut("/", UpdateHandler);
    }

    private static Task<Results<Ok<DataCollectionDto<BlogDto>>, BadRequest<ProblemDetails>>> GetHandler(HttpContext context, IBlogStorageBroker blogStorageBroker)
    {
        return RunToOkAsync(Execute, context, "Failed to get blogs");

        async Task<DataCollectionDto<BlogDto>> Execute()
        {
            var blogs = await blogStorageBroker.GetAsync();

            var dtos = blogs.Select(BlogMapper.MapToDto);

            return dtos.ToDataCollectionDto();
        }
    }

    private static Task<Results<Ok<BlogDto>, BadRequest<ProblemDetails>>> GetByIdHandler(HttpContext context, IBlogStorageBroker blogStorageBroker, long id)
    {
        return RunToOkAsync(Execute, context, "Failed to get blog by id");

        async Task<BlogDto> Execute()
        {
            var blog = await blogStorageBroker.GetByIdAsync(id.ToId());

            var dto = BlogMapper.MapToDto(blog);

            return dto;
        }
    }

    private static Task<Results<Created<BlogDto>, BadRequest<ProblemDetails>>> CreateHandler(HttpContext context, IBlogStorageBroker blogStorageBroker, BlogDto blogDto)
    {
        return RunAsync(Execute, context, "Failed to create blog");

        async Task<Created<BlogDto>> Execute()
        {
            var blog = new Blog(null, blogDto.Url);

            var addedBlog = await blogStorageBroker.AddAsync(blog);

            var dto = BlogMapper.MapToDto(addedBlog);

            return dto.ToCreated(context);
        }
    }

    private static Task<Results<Ok, BadRequest<ProblemDetails>>> DeleteHandler(HttpContext context, IBlogStorageBroker blogStorageBroker, long id)
    {
        return RunToOkAsync(Execute, context, $"Failed to delete blog by id '{id}'");

        async Task Execute()
        {
            await blogStorageBroker.DeleteAsync(id.ToId());
        }
    }

    private static Task<Results<Ok<BlogDto>, BadRequest<ProblemDetails>>> UpdateHandler(HttpContext context, IBlogStorageBroker blogStorageBroker, BlogDto blogDto)
    {
        return RunToOkAsync(Execute, context, $"Failed to update blog with id '{blogDto.Id}'");

        async Task<BlogDto> Execute()
        {
            var blog = new Blog(blogDto.Id.ToId(), blogDto.Url);

            var updatedBlog = await blogStorageBroker.UpdateAsync(blog);

            var dto = BlogMapper.MapToDto(updatedBlog);

            return dto;
        }
    }
}
