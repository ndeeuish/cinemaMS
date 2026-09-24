using FluentValidation;

namespace CinemaMS.Application.Features.Articles.Commands;

public class CreateArticleCommandValidator : AbstractValidator<CreateArticleCommand>
{
    public CreateArticleCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Summary).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Content).NotEmpty();
        RuleFor(x => x.ImageUrl).NotEmpty();
        RuleFor(x => x.Type).IsInEnum();
    }
}
