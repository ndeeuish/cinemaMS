using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CinemaMS.Domain.Common;
using CinemaMS.Domain.Entities.Cinemas;

namespace CinemaMS.Domain.Entities.Bookings;

[Table("Tickets")]
public class Ticket : EntityBase
{
    public int BookingId { get; set; }
    public int ShowtimeId { get; set; }
    public int SeatId { get; set; }
    public decimal Price { get; set; }

    //Fk
    [ForeignKey(nameof(BookingId))]
    public Booking Booking { get; set; } = default!;

    [ForeignKey(nameof(ShowtimeId))]
    public Showtime Showtime { get; set; } = default!;

    [ForeignKey(nameof(SeatId))]
    public Seat Seat { get; set; } = default!;

}
