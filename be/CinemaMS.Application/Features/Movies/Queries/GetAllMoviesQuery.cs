using CinemaMS.Application.Features.Common.Pagination;
using CinemaMS.Application.Features.Movies.DTOs;
using MediatR;

namespace CinemaMS.Application.Features.Movies.Queries;

public class GetAllMoviesQuery : FilterBase, IRequest<PagedResult<MovieDto>>
{
    public int? GenreId { get; set; }
}
