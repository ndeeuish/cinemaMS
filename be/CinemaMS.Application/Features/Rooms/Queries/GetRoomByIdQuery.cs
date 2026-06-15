using CinemaMS.Application.Features.Rooms.DTOs;
using CinemaMS.Application.Messaging;

namespace CinemaMS.Application.Features.Rooms.Queries;

public class GetRoomByIdQuery : QueryBase<RoomDto>
{
    public int Id { get; set; }
    public GetRoomByIdQuery(int id) => Id = id;
}
