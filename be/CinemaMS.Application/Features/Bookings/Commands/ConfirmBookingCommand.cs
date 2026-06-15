using CinemaMS.Application.Messaging;
using System.Text.Json.Serialization;

namespace CinemaMS.Application.Features.Bookings.Commands;

public class ConfirmBookingCommand : CommandBase<bool>
{
    [JsonIgnore]
    public int UserId { get; set; }
    public int BookingId { get; set; }
}
