using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CinemaMS.Domain.Common;

namespace CinemaMS.Domain.Entities.Bookings;

[Table("Payments")]
public class Payment : EntityBase
{
    public int BookingId { get; set; }
    public decimal Amount { get; set; }

    [Required]
    [MaxLength(50)]
    public string PaymentMethod { get; set; } = default!;

    [MaxLength(200)]
    public string TransactionCode { get; set; } = default!;

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = default!; // Success, Failed

    //Fk
    [ForeignKey(nameof(BookingId))]
    public Booking Booking { get; set; } = default!;
}
