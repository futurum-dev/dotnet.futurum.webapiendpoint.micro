using FluentValidation;

namespace Futurum.WebApiEndpoint.Micro.Sample.Blog;

public class BlogDtoValidator : AbstractValidator<BlogDto>
{
    public BlogDtoValidator()
    {
        RuleFor(x => x.Url).NotEmpty().WithMessage("must have a value;");
    }
}