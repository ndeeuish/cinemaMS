using CinemaMS.Domain.Entities.Bookings;

namespace CinemaMS.Application.Features.Bookings.DTOs;

public class TicketDto
{
    public int Id { get; set; }
    public int ShowtimeId { get; set; }
    public string MovieTitle { get; set; } = default!;
    public string SeatCode { get; set; } = default!;
    public decimal Price { get; set; }

    public static TicketDto FromEntity(Ticket ticket)
    {
        return new TicketDto
        {
            Id = ticket.Id,
            ShowtimeId = ticket.ShowtimeId,
            MovieTitle = ticket.Showtime?.Movie?.Title ?? "Unknown",
            SeatCode = ticket.Seat != null ? $"{ticket.Seat.RowIndex}{ticket.Seat.SeatNumber}" : "Unknown",
            Price = ticket.Price
        };
    }
}
