using CinemaMS.Application.Features.Rooms.DTOs;
using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Entities.Cinemas;
using CinemaMS.Domain.Exceptions;
using CinemaMS.Domain.Repositories;

namespace CinemaMS.Application.Features.Rooms.Queries;

public class GetRoomByIdQueryHandler : QueryHandlerBase<GetRoomByIdQuery, RoomDto>
{
    private readonly IRoomRepository _roomRepository;

    public GetRoomByIdQueryHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public override async Task<RoomDto> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.GetByIdAsync(request.Id, cancellationToken);
        if (room == null)
            throw new NotFoundException(nameof(Room), request.Id);

        return RoomDto.FromEntity(room);
    }
}
