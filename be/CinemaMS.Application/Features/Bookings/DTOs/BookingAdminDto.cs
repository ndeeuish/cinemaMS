using CinemaMS.Domain.Entities.Bookings;

namespace CinemaMS.Application.Features.Bookings.DTOs;

public class BookingAdminDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = default!;
    public string UserEmail { get; set; } = default!;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    
    // Summary info
    public string MovieTitle { get; set; } = default!;
    public string CinemaName { get; set; } = default!;
    public string RoomName { get; set; } = default!;
    public DateTime? ShowtimeStart { get; set; }
    public int TicketCount { get; set; }
    public string SeatCodes { get; set; } = default!;

    public static BookingAdminDto FromEntity(Booking booking)
    {
        var firstTicket = booking.Tickets.FirstOrDefault();
        var showtime = firstTicket?.Showtime;

        return new BookingAdminDto
        {
            Id = booking.Id,
            UserId = booking.UserId,
            UserName = booking.User?.FullName ?? "Unknown",
            UserEmail = booking.User?.Email ?? "Unknown",
            TotalAmount = booking.TotalAmount,
            Status = booking.Status,
            CreatedAt = booking.CreatedAt,
            MovieTitle = showtime?.Movie?.Title ?? "N/A",
            CinemaName = showtime?.Room?.Cinema?.Name ?? "N/A",
            RoomName = showtime?.Room?.Name ?? "N/A",
            ShowtimeStart = showtime?.StartTime,
            TicketCount = booking.Tickets.Count,
            SeatCodes = string.Join(", ", booking.Tickets.Select(t => t.Seat != null ? $"{t.Seat.RowIndex}{t.Seat.SeatNumber}" : ""))
        };
    }
}
