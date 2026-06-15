using CinemaMS.Application.Features.Rooms.DTOs;
using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Entities.Cinemas;
using CinemaMS.Domain.Exceptions;
using CinemaMS.Domain.Repositories;

namespace CinemaMS.Application.Features.Rooms.Commands;

public class UpdateRoomCommandHandler : CommandHandlerBase<UpdateRoomCommand, RoomDto>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRoomCommandHandler(IRoomRepository roomRepository, IUnitOfWork unitOfWork)
    {
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<RoomDto> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.GetByIdAsync(request.Id, cancellationToken);
        if (room == null)
            throw new NotFoundException(nameof(Room), request.Id);

        if (room.Name != request.Name && await _roomRepository.ExistsByNameInCinemaAsync(request.Name, room.CinemaId, cancellationToken))
        {
            throw new UserFriendlyException($"Room name '{request.Name}' already exists in this cinema.");
        }

        room.Name = request.Name;
        room.Capacity = request.Capacity;
        room.RoomTypeId = request.RoomTypeId;

        _roomRepository.Update(room);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return RoomDto.FromEntity(room);
    }
}
