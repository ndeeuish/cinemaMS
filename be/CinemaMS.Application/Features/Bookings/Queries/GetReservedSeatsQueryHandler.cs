using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Repositories;

namespace CinemaMS.Application.Features.Bookings.Queries;

public class GetReservedSeatsQueryHandler : QueryHandlerBase<GetReservedSeatsQuery, IEnumerable<int>>
{
    private readonly IBookingRepository _bookingRepository;

    public GetReservedSeatsQueryHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public override async Task<IEnumerable<int>> Handle(GetReservedSeatsQuery request, CancellationToken cancellationToken)
    {
        return await _bookingRepository.GetReservedSeatIdsAsync(request.ShowtimeId, cancellationToken);
    }
}
