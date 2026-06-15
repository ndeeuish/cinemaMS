using CinemaMS.Domain.Entities.Bookings;

namespace CinemaMS.Application.Features.Showtimes.DTOs;

public class ShowtimeDto
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public string MovieTitle { get; set; } = default!;
    public int RoomId { get; set; }
    public string RoomName { get; set; } = default!;
    public string CinemaName { get; set; } = default!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal BasePrice { get; set; }

    public static ShowtimeDto FromEntity(Showtime showtime)
    {
        return new ShowtimeDto
        {
            Id = showtime.Id,
            MovieId = showtime.MovieId,
            MovieTitle = showtime.Movie?.Title ?? "Unknown",
            RoomId = showtime.RoomId,
            RoomName = showtime.Room?.Name ?? "Unknown",
            CinemaName = showtime.Room?.Cinema?.Name ?? "Unknown",
            StartTime = showtime.StartTime,
            EndTime = showtime.EndTime,
            BasePrice = showtime.BasePrice + (showtime.Room?.RoomType?.Surcharge ?? 0)
        };
    }
}
