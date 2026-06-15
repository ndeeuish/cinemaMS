using CinemaMS.Application.Features.Common.Pagination;
using CinemaMS.Application.Features.Showtimes.DTOs;
using MediatR;

namespace CinemaMS.Application.Features.Showtimes.Queries;

public class GetShowtimesByCinemaIdQuery : FilterBase, IRequest<PagedResult<ShowtimeDto>>
{
    public int CinemaId { get; set; }

    public GetShowtimesByCinemaIdQuery(int cinemaId)
    {
        CinemaId = cinemaId;
    }
    
    // For model binding from query string
    public GetShowtimesByCinemaIdQuery() { }
}
