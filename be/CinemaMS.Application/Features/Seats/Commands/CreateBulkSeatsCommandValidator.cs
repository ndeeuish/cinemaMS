using FluentValidation;

namespace CinemaMS.Application.Features.Seats.Commands;

public class CreateBulkSeatsCommandValidator : AbstractValidator<CreateBulkSeatsCommand>
{
    public CreateBulkSeatsCommandValidator()
    {
        RuleFor(x => x.RoomId).GreaterThan(0);
        RuleFor(x => x.Matrix)
            .NotEmpty().WithMessage("Matrix cannot be empty.")
            .Must(m => m.Length <= 26).WithMessage("Maximum of 26 rows supported (A-Z).");
    }
}
