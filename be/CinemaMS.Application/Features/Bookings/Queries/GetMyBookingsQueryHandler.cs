using CinemaMS.Application.Features.Bookings.DTOs;
using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Repositories;

namespace CinemaMS.Application.Features.Bookings.Queries;

public class GetMyBookingsQueryHandler : QueryHandlerBase<GetMyBookingsQuery, IEnumerable<BookingDto>>
{
    private readonly IBookingRepository _bookingRepository;

    public GetMyBookingsQueryHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public override async Task<IEnumerable<BookingDto>> Handle(GetMyBookingsQuery request, CancellationToken cancellationToken)
    {
        var bookings = await _bookingRepository.GetUserBookingsAsync(request.UserId, cancellationToken);
        return bookings.Select(BookingDto.FromEntity);
    }
}
