using CinemaMS.Application.Features.Rooms.DTOs;
using CinemaMS.Application.Messaging;

namespace CinemaMS.Application.Features.Rooms.Commands;

public class CreateRoomCommand : CommandBase<RoomDto>
{
    public string Name { get; set; } = default!;
    public int Capacity { get; set; }
    public int CinemaId { get; set; }
    public int RoomTypeId { get; set; }
}
