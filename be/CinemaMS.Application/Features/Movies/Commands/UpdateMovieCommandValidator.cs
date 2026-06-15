using FluentValidation;

namespace CinemaMS.Application.Features.Movies.Commands;

public class UpdateMovieCommandValidator : AbstractValidator<UpdateMovieCommand>
{
    public UpdateMovieCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.DurationInMinutes).GreaterThan(0);
        RuleFor(x => x.AgeRestrictionId).GreaterThan(0);
    }
}
