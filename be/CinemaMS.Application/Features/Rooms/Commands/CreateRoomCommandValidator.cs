using FluentValidation;

namespace CinemaMS.Application.Features.Rooms.Commands;

public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Capacity).GreaterThan(0);
        RuleFor(x => x.CinemaId).GreaterThan(0);
        RuleFor(x => x.RoomTypeId).GreaterThan(0);
    }
}
