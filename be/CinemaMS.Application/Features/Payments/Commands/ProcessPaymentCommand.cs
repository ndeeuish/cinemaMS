using CinemaMS.Application.Features.Payments.DTOs;
using CinemaMS.Application.Messaging;

namespace CinemaMS.Application.Features.Payments.Commands;

public class ProcessPaymentCommand : CommandBase<PaymentDto>
{
    public int BookingId { get; set; }
    public string PaymentMethod { get; set; } = default!;
    public string TransactionCode { get; set; } = default!;
}
