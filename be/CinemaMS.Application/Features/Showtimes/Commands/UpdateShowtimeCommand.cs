using CinemaMS.Application.Features.Showtimes.DTOs;
using CinemaMS.Application.Messaging;
using System.Text.Json.Serialization;

namespace CinemaMS.Application.Features.Showtimes.Commands;

public class UpdateShowtimeCommand : CommandBase<ShowtimeDto>
{
    [JsonIgnore]
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public decimal BasePrice { get; set; }
}
