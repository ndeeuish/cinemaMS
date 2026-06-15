using CinemaMS.Application.Features.Showtimes.DTOs;
using CinemaMS.Application.Messaging;

namespace CinemaMS.Application.Features.Showtimes.Commands;

public class CreateShowtimeCommand : CommandBase<ShowtimeDto>
{
    public int MovieId { get; set; }
    public int RoomId { get; set; }
    public DateTime StartTime { get; set; }
    public decimal BasePrice { get; set; }
}
