using CinemaMS.Application.Features.Movies.DTOs;
using CinemaMS.Application.Messaging;

namespace CinemaMS.Application.Features.Movies.Queries;

public class GetMovieByIdQuery : QueryBase<MovieDto>
{
    public int Id { get; set; }
    public GetMovieByIdQuery(int id) => Id = id;
}
