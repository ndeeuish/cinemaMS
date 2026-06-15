using CinemaMS.Application.Features.Bookings.DTOs;
using CinemaMS.Application.Messaging;

namespace CinemaMS.Application.Features.Bookings.Queries;

public class GetMyBookingsQuery : QueryBase<IEnumerable<BookingDto>>
{
    public int UserId { get; set; }
    public GetMyBookingsQuery(int userId) => UserId = userId;
}
