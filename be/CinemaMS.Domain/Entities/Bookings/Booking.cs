using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CinemaMS.Domain.Common;
using CinemaMS.Domain.Entities.Identity;

namespace CinemaMS.Domain.Entities.Bookings;

[Table("Bookings")]
public class Booking : EntityBase
{
    public int UserId { get; set; }
    public decimal TotalAmount { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = default!; // Holding, PendingPayment, Confirmed, Canceled

    public DateTime HoldExpiration { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = default!;

    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
