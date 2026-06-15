using CinemaMS.Application.Features.Rooms.DTOs;
using CinemaMS.Application.Messaging;
using System.Text.Json.Serialization;

namespace CinemaMS.Application.Features.Rooms.Commands;

public class UpdateRoomCommand : CommandBase<RoomDto>
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public int Capacity { get; set; }
    public int RoomTypeId { get; set; }
}
