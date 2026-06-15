using CinemaMS.Application.Features.Bookings.DTOs;
using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Entities.Bookings;
using CinemaMS.Domain.Exceptions;
using CinemaMS.Domain.Repositories;
using CinemaMS.Application.Interfaces.Caching;

namespace CinemaMS.Application.Features.Bookings.Commands;

public class CreateBookingCommandHandler : CommandHandlerBase<CreateBookingCommand, BookingDto>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IShowtimeRepository _showtimeRepository;
    private readonly ISeatRepository _seatRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisCacheService _cacheService;

    public CreateBookingCommandHandler(
        IBookingRepository bookingRepository,
        IShowtimeRepository showtimeRepository,
        ISeatRepository seatRepository,
        IUnitOfWork unitOfWork,
        IRedisCacheService cacheService)
    {
        _bookingRepository = bookingRepository;
        _showtimeRepository = showtimeRepository;
        _seatRepository = seatRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public override async Task<BookingDto> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var showtime = await _showtimeRepository.GetByIdWithDetailsAsync(request.ShowtimeId, cancellationToken);
        if (showtime == null) throw new NotFoundException(nameof(Showtime), request.ShowtimeId);

        // 0. Distributed Cache Lock (Redis) Check
        var lockKeys = request.SeatIds.Select(id => $"SeatLock_{request.ShowtimeId}_{id}").ToList();
        var acquiredLocks = new List<string>();

        try
        {
            foreach (var key in lockKeys)
            {
                var existing = await _cacheService.GetAsync<string>(key, cancellationToken);
                if (existing != null)
                {
                    throw new UserFriendlyException("One or more selected seats are currently being held by someone else.");
                }
                
                await _cacheService.SetAsync(key, "locked", absoluteExpireTime: TimeSpan.FromMinutes(10), cancellationToken: cancellationToken);
                acquiredLocks.Add(key);
            }

            // 1. Conflict Check (SQL DB as fallback)
            var reservedSeats = await _bookingRepository.GetReservedSeatIdsAsync(request.ShowtimeId, cancellationToken);
            var conflictSeats = request.SeatIds.Intersect(reservedSeats).ToList();
            if (conflictSeats.Any())
            {
                throw new UserFriendlyException("One or more selected seats have already been booked or are currently held in Database.");
            }

            // 2. Fetch seats with types to calculate total price
            var allSeatsInRoom = await _seatRepository.GetSeatsByRoomIdAsync(showtime.RoomId, cancellationToken);
            var selectedSeats = allSeatsInRoom.Where(s => request.SeatIds.Contains(s.Id)).ToList();
            
            if (selectedSeats.Count != request.SeatIds.Count)
                throw new UserFriendlyException("Invalid seats provided or they do not belong to this room.");

            // 3. Create Booking
            var booking = new Booking
            {
                UserId = request.UserId,
                Status = "Holding",
                HoldExpiration = DateTime.UtcNow.AddMinutes(10),
                TotalAmount = 0
            };

            decimal calculatedTotal = 0;
            foreach (var seat in selectedSeats)
            {
                decimal ticketPrice = showtime.BasePrice + (showtime.Room?.RoomType?.Surcharge ?? 0) + (seat.SeatType?.Surcharge ?? 0);
                calculatedTotal += ticketPrice;

                booking.Tickets.Add(new Ticket
                {
                    ShowtimeId = showtime.Id,
                    SeatId = seat.Id,
                    Price = ticketPrice
                });
            }

            booking.TotalAmount = calculatedTotal;

            _bookingRepository.Add(booking);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Fetch completely to return mapped DTO
            var createdBooking = await _bookingRepository.GetByIdWithDetailsAsync(booking.Id, cancellationToken);
            return BookingDto.FromEntity(createdBooking!);
        }
        catch
        {
            // Release all acquired locks if transaction fails
            foreach (var key in acquiredLocks)
            {
                await _cacheService.RemoveAsync(key, CancellationToken.None);
            }
            throw;
        }
    }
}
