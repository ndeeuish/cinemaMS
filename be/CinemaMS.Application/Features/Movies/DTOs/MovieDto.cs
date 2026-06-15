using CinemaMS.Domain.Entities.Catalog;

namespace CinemaMS.Application.Features.Movies.DTOs;

public class MovieDto
{
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
    public string AgeRestrictionCode { get; set; } = default!;
    
    public List<int> GenreIds { get; set; } = new();
    public List<string> Genres { get; set; } = new();

    public static MovieDto FromEntity(Movie movie)
    {
        return new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            DurationInMinutes = movie.DurationInMinutes,
            ReleaseDate = movie.ReleaseDate,
            Director = movie.Director,
            Casts = movie.Casts,
            PosterUrl = movie.PosterUrl,
            TrailerUrl = movie.TrailerUrl,
            AgeRestrictionId = movie.AgeRestrictionId,
            AgeRestrictionCode = movie.AgeRestriction?.Code ?? "UNKNOWN",
            GenreIds = movie.MovieGenres?.Select(mg => mg.GenreId).ToList() ?? new List<int>(),
            Genres = movie.MovieGenres?.Select(mg => mg.Genre?.Name ?? "Unknown").ToList() ?? new List<string>()
        };
    }
}
