using CinemaMS.Application.Features.Rooms.DTOs;
using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Entities.Cinemas;
using CinemaMS.Domain.Exceptions;
using CinemaMS.Domain.Repositories;

namespace CinemaMS.Application.Features.Rooms.Commands;

public class CreateRoomCommandHandler : CommandHandlerBase<CreateRoomCommand, RoomDto>
{
    private readonly IRoomRepository _roomRepository;
    private readonly ICinemaRepository _cinemaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoomCommandHandler(
        IRoomRepository roomRepository,
        ICinemaRepository cinemaRepository,
        IUnitOfWork unitOfWork)
    {
        _roomRepository = roomRepository;
        _cinemaRepository = cinemaRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<RoomDto> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var cinema = await _cinemaRepository.GetByIdAsync(request.CinemaId, cancellationToken);
        if (cinema == null)
            throw new NotFoundException(nameof(Cinema), request.CinemaId);

        if (await _roomRepository.ExistsByNameInCinemaAsync(request.Name, request.CinemaId, cancellationToken))
            throw new UserFriendlyException($"Room name '{request.Name}' already exists in this cinema.");

        var room = new Room
        {
            Name = request.Name,
            Capacity = request.Capacity,
            CinemaId = request.CinemaId,
            RoomTypeId = request.RoomTypeId
        };

        _roomRepository.Add(room);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return RoomDto.FromEntity(room);
    }
}
