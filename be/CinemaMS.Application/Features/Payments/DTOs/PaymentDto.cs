using CinemaMS.Domain.Entities.Bookings;

namespace CinemaMS.Application.Features.Payments.DTOs;

public class PaymentDto
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = default!;
    public string TransactionCode { get; set; } = default!;
    public string Status { get; set; } = default!;
    public DateTime CreatedAt { get; set; }

    public static PaymentDto FromEntity(Payment payment)
    {
        return new PaymentDto
        {
            Id = payment.Id,
            BookingId = payment.BookingId,
            Amount = payment.Amount,
            PaymentMethod = payment.PaymentMethod,
            TransactionCode = payment.TransactionCode,
            Status = payment.Status,
            CreatedAt = payment.CreatedAt
        };
    }
}
