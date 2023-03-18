using System.ComponentModel.DataAnnotations;

using FluentValidation;

namespace Futurum.WebApiEndpoint.Micro.Sample.Features;

[WebApiEndpoint("validation")]
public class ValidationWebApiEndpoint : IWebApiEndpoint
{
    public void Configure(RouteGroupBuilder groupBuilder, WebApiEndpointVersion webApiEndpointVersion)
    {
    }

    public void Register(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/fluent-validation", FluentValidationHandler);

        builder.MapPost("/data-annotations-validation", DataAnnotationsValidationHandler);

        builder.MapPost("/validation", ValidationHandler);
    }

    private static Results<Ok<ArticleDto>, ValidationProblem, BadRequest<ProblemDetails>> FluentValidationHandler(HttpContext context, IFluentValidationService<ArticleDto> fluentValidationService,
                                                                                                                  ArticleDto articleDto) =>
        fluentValidationService.Execute(articleDto)
                               .Map(() => new Article(null, articleDto.Url))
                               .Map(ArticleMapper.MapToDto)
                               .ToWebApi(context, ToOk, ToValidationProblem);

    private static Results<Ok<ArticleDto>, ValidationProblem, BadRequest<ProblemDetails>> DataAnnotationsValidationHandler(HttpContext context,
                                                                                                                           IDataAnnotationsValidationService dataAnnotationsValidationService,
                                                                                                                           ArticleDto articleDto) =>
        dataAnnotationsValidationService.Execute(articleDto)
                                        .Map(() => new Article(null, articleDto.Url))
                                        .Map(ArticleMapper.MapToDto)
                                        .ToWebApi(context, ToOk, ToValidationProblem);

    private static Results<Ok<ArticleDto>, ValidationProblem, BadRequest<ProblemDetails>> ValidationHandler(HttpContext context, IValidationService<ArticleDto> validationService,
                                                                                                            ArticleDto articleDto) =>
        validationService.Execute(articleDto)
                         .Map(() => new Article(null, articleDto.Url))
                         .Map(ArticleMapper.MapToDto)
                         .ToWebApi(context, ToOk, ToValidationProblem);

    public class ArticleDtoValidator : AbstractValidator<ArticleDto>
    {
        public ArticleDtoValidator()
        {
            RuleFor(x => x.Url).NotEmpty().WithMessage("must have a value;");
        }
    }

    public record ArticleDto(long Id,
                             [property: Required, StringLength(50, MinimumLength = 3)]
                             string Url);

    public record Article(long? Id, string Url);

    public static class ArticleMapper
    {
        public static ArticleDto MapToDto(Article domain) =>
            new(domain.Id ?? 0, domain.Url);
    }
}