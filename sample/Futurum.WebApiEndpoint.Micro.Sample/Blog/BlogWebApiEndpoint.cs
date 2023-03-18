namespace Futurum.WebApiEndpoint.Micro.Sample.Blog;

[WebApiEndpoint("blog")]
public class BlogWebApiEndpoint : IWebApiEndpoint
{
    public void Configure(RouteGroupBuilder groupBuilder, WebApiEndpointVersion webApiEndpointVersion)
    {
    }

    public void Register(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/", GetHandler);

        builder.MapGet("/{id}", GetByIdHandler);

        builder.MapPost("/", CreateHandler);

        builder.MapDelete("/{id}", DeleteHandler);

        builder.MapPut("/", UpdateHandler);
    }

    private static Task<Results<Ok<DataCollectionDto<BlogDto>>, BadRequest<ProblemDetails>>> GetHandler(HttpContext context, IBlogStorageBroker blogStorageBroker) =>
        blogStorageBroker.GetAsync()
                         .MapAsAsync(BlogMapper.MapToDto)
                         .MapAsync(DataCollectionExtensions.ToDataCollectionDto)
                         .ToWebApiAsync(context, ToOk);

    private static Task<Results<Ok<BlogDto>, BadRequest<ProblemDetails>>> GetByIdHandler(HttpContext context, IBlogStorageBroker blogStorageBroker, long id) =>
        blogStorageBroker.GetByIdAsync(id.ToId())
                         .MapAsync(BlogMapper.MapToDto)
                         .ToWebApiAsync(context, ToOk);

    private static Task<Results<Created<BlogDto>, ValidationProblem, BadRequest<ProblemDetails>>> CreateHandler(HttpContext context, IBlogStorageBroker blogStorageBroker,
                                                                                                                IValidationService<BlogDto> validationService,
                                                                                                                BlogDto blogDto) =>
        validationService.Execute(blogDto)
                         .Map(() => new Blog(Option<Id>.None, blogDto.Url))
                         .ThenAsync(blogStorageBroker.AddAsync)
                         .MapAsync(BlogMapper.MapToDto)
                         .ToWebApiAsync(context, ToCreated, ToValidationProblem);

    private static Task<Results<Ok, BadRequest<ProblemDetails>>> DeleteHandler(HttpContext context, IBlogStorageBroker blogStorageBroker, long id) =>
        blogStorageBroker.DeleteAsync(id.ToId())
                         .ToWebApiAsync(context, ToOk);

    private static Task<Results<Ok<BlogDto>, BadRequest<ProblemDetails>>> UpdateHandler(HttpContext context, IBlogStorageBroker blogStorageBroker,
                                                                                        IValidationService<BlogDto> validationService,
                                                                                        BlogDto blogDto) =>
        validationService.Execute(blogDto)
                         .Map(() => new Blog(blogDto.Id.ToId(), blogDto.Url))
                         .ThenAsync(blogStorageBroker.UpdateAsync)
                         .MapAsync(BlogMapper.MapToDto)
                         .ToWebApiAsync(context, ToOk);
}