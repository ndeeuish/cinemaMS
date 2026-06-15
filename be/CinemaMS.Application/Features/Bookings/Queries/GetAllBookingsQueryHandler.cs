using CinemaMS.Application.Features.Bookings.DTOs;
using CinemaMS.Application.Features.Common.Pagination;
using CinemaMS.Domain.Repositories;
using MediatR;

namespace CinemaMS.Application.Features.Bookings.Queries;

public class GetAllBookingsQueryHandler : IRequestHandler<GetAllBookingsQuery, PagedResult<BookingAdminDto>>
{
    private readonly IBookingRepository _bookingRepository;

    public GetAllBookingsQueryHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<PagedResult<BookingAdminDto>> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _bookingRepository.GetPagedAsync(
            request.PageIndex,
            request.PageSize,
            request.Keyword,
            cancellationToken);

        return new PagedResult<BookingAdminDto>
        {
            Items = items.Select(BookingAdminDto.FromEntity),
            TotalItems = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }
}
