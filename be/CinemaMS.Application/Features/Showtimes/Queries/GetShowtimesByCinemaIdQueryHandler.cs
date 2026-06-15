using CinemaMS.Application.Features.Common.Pagination;
using CinemaMS.Application.Features.Showtimes.DTOs;
using CinemaMS.Domain.Repositories;
using MediatR;

namespace CinemaMS.Application.Features.Showtimes.Queries;

public class GetShowtimesByCinemaIdQueryHandler : IRequestHandler<GetShowtimesByCinemaIdQuery, PagedResult<ShowtimeDto>>
{
    private readonly IShowtimeRepository _showtimeRepository;

    public GetShowtimesByCinemaIdQueryHandler(IShowtimeRepository showtimeRepository)
    {
        _showtimeRepository = showtimeRepository;
    }

    public async Task<PagedResult<ShowtimeDto>> Handle(GetShowtimesByCinemaIdQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _showtimeRepository.GetPagedByCinemaIdAsync(
            request.CinemaId,
            request.PageIndex,
            request.PageSize,
            request.Keyword,
            cancellationToken);

        return new PagedResult<ShowtimeDto>
        {
            Items = items.Select(ShowtimeDto.FromEntity),
            TotalItems = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }
}
