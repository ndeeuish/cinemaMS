using CinemaMS.Application.Features.Movies.DTOs;
using CinemaMS.Application.Messaging;

namespace CinemaMS.Application.Features.Movies.Commands;

public class CreateMovieCommand : CommandBase<MovieDto>
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int DurationInMinutes { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Director { get; set; } = default!;
    public string Casts { get; set; } = default!;
    public string PosterUrl { get; set; } = default!;
    public string TrailerUrl { get; set; } = default!;
    public int AgeRestrictionId { get; set; }
    public List<int> GenreIds { get; set; } = new();
}
