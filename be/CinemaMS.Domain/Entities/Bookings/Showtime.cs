using System.ComponentModel.DataAnnotations.Schema;
using CinemaMS.Domain.Common;
using CinemaMS.Domain.Entities.Catalog;
using CinemaMS.Domain.Entities.Cinemas;

namespace CinemaMS.Domain.Entities.Bookings;

[Table("Showtimes")]
public class Showtime : EntityBase
{
    public int MovieId { get; set; }
    public int RoomId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal BasePrice { get; set; }

    // Fk
    [ForeignKey(nameof(MovieId))]
    public Movie Movie { get; set; } = default!;

    [ForeignKey(nameof(RoomId))]
    public Room Room { get; set; } = default!;
}
