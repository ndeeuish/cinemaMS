using CinemaMS.Application.Features.Bookings.DTOs;
using CinemaMS.Application.Messaging;
using System.Text.Json.Serialization;

namespace CinemaMS.Application.Features.Bookings.Commands;

public class CreateBookingCommand : CommandBase<BookingDto>
{
    [JsonIgnore]
    public int UserId { get; set; }
    public int ShowtimeId { get; set; }
    public List<int> SeatIds { get; set; } = new();
}
