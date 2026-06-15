using FluentValidation;

namespace CinemaMS.Application.Features.Rooms.Commands;

public class UpdateRoomCommandValidator : AbstractValidator<UpdateRoomCommand>
{
    public UpdateRoomCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Capacity).GreaterThan(0);
        RuleFor(x => x.RoomTypeId).GreaterThan(0);
    }
}
