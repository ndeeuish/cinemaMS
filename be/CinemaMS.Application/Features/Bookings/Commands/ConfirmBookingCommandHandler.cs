using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Entities.Bookings;
using CinemaMS.Domain.Exceptions;
using CinemaMS.Domain.Repositories;

namespace CinemaMS.Application.Features.Bookings.Commands;

public class ConfirmBookingCommandHandler : CommandHandlerBase<ConfirmBookingCommand, bool>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmBookingCommandHandler(IBookingRepository bookingRepository, IUnitOfWork unitOfWork)
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<bool> Handle(ConfirmBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(request.BookingId, cancellationToken);
        
        if (booking == null) 
            throw new NotFoundException(nameof(Booking), request.BookingId);

        if (booking.UserId != request.UserId)
            throw new UnauthorizedException("You are not authorized to confirm this booking.");

        if (booking.Status == "Confirmed")
            throw new UserFriendlyException("Booking is already confirmed.");

        if (booking.Status != "Holding")
            throw new UserFriendlyException($"Booking cannot be confirmed from status: {booking.Status}");

        if (DateTime.UtcNow > booking.HoldExpiration)
        {
            booking.Status = "Canceled";
            _bookingRepository.Update(booking);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            throw new UserFriendlyException("Booking hold time has expired. Your reservation has been canceled.");
        }

        booking.Status = "Confirmed";
        _bookingRepository.Update(booking);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
