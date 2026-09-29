using CinemaMS.Application.Features.Movies.DTOs;
using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Entities.Catalog;
using CinemaMS.Domain.Repositories;

namespace CinemaMS.Application.Features.Movies.Commands;

public class CreateMovieCommandHandler : CommandHandlerBase<CreateMovieCommand, MovieDto>
{
    private readonly IMovieRepository _movieRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CinemaMS.Application.Interfaces.Caching.IRedisCacheService _cacheService;

    public CreateMovieCommandHandler(IMovieRepository movieRepository, IUnitOfWork unitOfWork, CinemaMS.Application.Interfaces.Caching.IRedisCacheService cacheService)
    {
        _movieRepository = movieRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public override async Task<MovieDto> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = new Movie
        {
            Title = request.Title,
            Description = request.Description,
            DurationInMinutes = request.DurationInMinutes,
            ReleaseDate = request.ReleaseDate,
            Director = request.Director,
            Casts = request.Casts,
            PosterUrl = request.PosterUrl,
            TrailerUrl = request.TrailerUrl,
            AgeRestrictionId = request.AgeRestrictionId
        };

        if (request.GenreIds != null && request.GenreIds.Any())
        {
            foreach (var genreId in request.GenreIds)
            {
                movie.MovieGenres.Add(new MovieGenre { GenreId = genreId });
            }
        }

        _movieRepository.Add(movie);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveAsync("Movies_1_10_all_all", cancellationToken);

        // Fetch back with details included to map correct DTO
        var createdMovie = await _movieRepository.GetByIdWithDetailsAsync(movie.Id, cancellationToken);
        return MovieDto.FromEntity(createdMovie!);
    }
}
