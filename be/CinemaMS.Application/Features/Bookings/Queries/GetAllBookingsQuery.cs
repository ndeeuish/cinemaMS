using CinemaMS.Application.Features.Bookings.DTOs;
using CinemaMS.Application.Features.Common.Pagination;
using MediatR;

namespace CinemaMS.Application.Features.Bookings.Queries;

public class GetAllBookingsQuery : FilterBase, IRequest<PagedResult<BookingAdminDto>>
{
}
