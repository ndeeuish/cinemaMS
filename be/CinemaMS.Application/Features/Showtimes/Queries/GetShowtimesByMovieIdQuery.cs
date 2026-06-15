using CinemaMS.Application.Features.Showtimes.DTOs;
using CinemaMS.Application.Messaging;

namespace CinemaMS.Application.Features.Showtimes.Queries;

public class GetShowtimesByMovieIdQuery : QueryBase<IEnumerable<ShowtimeDto>>
{
    public int MovieId { get; set; }
    public GetShowtimesByMovieIdQuery(int movieId) => MovieId = movieId;
}
