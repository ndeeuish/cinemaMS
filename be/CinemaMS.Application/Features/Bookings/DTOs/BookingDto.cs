using CinemaMS.Domain.Entities.Bookings;

namespace CinemaMS.Application.Features.Bookings.DTOs;

public class BookingDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = default!;
    public DateTime HoldExpiration { get; set; }
    public DateTime CreatedAt { get; set; }
    public string MovieTitle { get; set; } = default!;
    public string CinemaName { get; set; } = default!;
    public string RoomName { get; set; } = default!;
    public DateTime? ShowtimeStart { get; set; }
    public List<TicketDto> Tickets { get; set; } = new();

    public static BookingDto FromEntity(Booking booking)
    {
        var firstTicket = booking.Tickets.FirstOrDefault();
        var showtime = firstTicket?.Showtime;

        return new BookingDto
        {
            Id = booking.Id,
            UserId = booking.UserId,
            TotalAmount = booking.TotalAmount,
            Status = booking.Status,
            HoldExpiration = booking.HoldExpiration,
            CreatedAt = booking.CreatedAt,
            MovieTitle = showtime?.Movie?.Title ?? "N/A",
            CinemaName = showtime?.Room?.Cinema?.Name ?? "N/A",
            RoomName = showtime?.Room?.Name ?? "N/A",
            ShowtimeStart = showtime?.StartTime,
            Tickets = booking.Tickets.Select(TicketDto.FromEntity).ToList()
        };
    }
}
