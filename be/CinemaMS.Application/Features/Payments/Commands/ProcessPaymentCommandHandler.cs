using CinemaMS.Application.Features.Payments.DTOs;
using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Entities.Bookings;
using CinemaMS.Domain.Exceptions;
using CinemaMS.Domain.Repositories;

namespace CinemaMS.Application.Features.Payments.Commands;

public class ProcessPaymentCommandHandler : CommandHandlerBase<ProcessPaymentCommand, PaymentDto>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProcessPaymentCommandHandler(
        IBookingRepository bookingRepository,
        IPaymentRepository paymentRepository,
        IUnitOfWork unitOfWork)
    {
        _bookingRepository = bookingRepository;
        _paymentRepository = paymentRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<PaymentDto> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(request.BookingId, cancellationToken);
        if (booking == null) throw new NotFoundException(nameof(Booking), request.BookingId);

        if (booking.Status == "Confirmed")
            throw new UserFriendlyException("This booking has already been paid and confirmed.");
            
        if (booking.Status == "Canceled")
            throw new UserFriendlyException("This booking was canceled.");

        if (booking.Status == "Holding" && booking.HoldExpiration <= DateTime.UtcNow)
        {
            booking.Status = "Canceled";
            _bookingRepository.Update(booking);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            throw new UserFriendlyException("Seat holding period has expired. Please try booking again.");
        }

        var payment = new Payment
        {
            BookingId = request.BookingId,
            Amount = booking.TotalAmount,
            PaymentMethod = request.PaymentMethod,
            TransactionCode = request.TransactionCode ?? "N/A",
            Status = "Success"
        };
        
        _paymentRepository.Add(payment);

        booking.Status = "Confirmed";
        _bookingRepository.Update(booking);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return PaymentDto.FromEntity(payment);
    }
}
