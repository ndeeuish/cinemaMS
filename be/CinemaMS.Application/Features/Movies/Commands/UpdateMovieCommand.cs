using CinemaMS.Application.Features.Movies.DTOs;
using CinemaMS.Application.Messaging;
using System.Text.Json.Serialization;

namespace CinemaMS.Application.Features.Movies.Commands;

public class UpdateMovieCommand : CommandBase<MovieDto>
{
    [JsonIgnore]
    public int Id { get; set; }
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
