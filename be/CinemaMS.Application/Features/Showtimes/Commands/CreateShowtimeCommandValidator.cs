using FluentValidation;

namespace CinemaMS.Application.Features.Showtimes.Commands;

public class CreateShowtimeCommandValidator : AbstractValidator<CreateShowtimeCommand>
{
    public CreateShowtimeCommandValidator()
    {
        RuleFor(x => x.MovieId).GreaterThan(0);
        RuleFor(x => x.RoomId).GreaterThan(0);
        RuleFor(x => x.StartTime).NotEmpty().GreaterThan(DateTime.UtcNow.AddMinutes(30))
            .WithMessage("Showtime must start at least 30 minutes from now.");
        RuleFor(x => x.BasePrice).GreaterThanOrEqualTo(0);
    }
}
