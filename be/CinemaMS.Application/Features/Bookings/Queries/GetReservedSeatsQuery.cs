using CinemaMS.Application.Messaging;

namespace CinemaMS.Application.Features.Bookings.Queries;

public class GetReservedSeatsQuery : QueryBase<IEnumerable<int>>
{
    public int ShowtimeId { get; set; }
    public GetReservedSeatsQuery(int showtimeId) => ShowtimeId = showtimeId;
}
