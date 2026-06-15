using CinemaMS.Domain.Entities.Cinemas;

namespace CinemaMS.Application.Features.Rooms.DTOs;

public class RoomDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public int Capacity { get; set; }
    public int CinemaId { get; set; }
    public int RoomTypeId { get; set; }
    public string RoomTypeName { get; set; } = default!;

    public static RoomDto FromEntity(Room room)
    {
        return new RoomDto
        {
            Id = room.Id,
            Name = room.Name,
            Capacity = room.Capacity,
            CinemaId = room.CinemaId,
            RoomTypeId = room.RoomTypeId,
            RoomTypeName = room.RoomType?.Name ?? "Unknown"
        };
    }
}
