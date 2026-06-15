using CinemaMS.Application.Features.Seats.DTOs;
using CinemaMS.Application.Messaging;

namespace CinemaMS.Application.Features.Seats.Queries;

public class GetSeatsByRoomIdQuery : QueryBase<IEnumerable<SeatDto>>
{
    public int RoomId { get; set; }
    public GetSeatsByRoomIdQuery(int roomId) => RoomId = roomId;
}
