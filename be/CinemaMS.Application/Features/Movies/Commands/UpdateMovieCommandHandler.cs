using CinemaMS.Application.Features.Movies.DTOs;
using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Entities.Catalog;
using CinemaMS.Domain.Exceptions;
using CinemaMS.Domain.Repositories;

namespace CinemaMS.Application.Features.Movies.Commands;

public class UpdateMovieCommandHandler : CommandHandlerBase<UpdateMovieCommand, MovieDto>
{
    private readonly IMovieRepository _movieRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMovieCommandHandler(IMovieRepository movieRepository, IUnitOfWork unitOfWork)
    {
        _movieRepository = movieRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<MovieDto> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);
        if (movie == null)
            throw new NotFoundException(nameof(Movie), request.Id);

        movie.Title = request.Title;
        movie.Description = request.Description;
        movie.DurationInMinutes = request.DurationInMinutes;
        movie.ReleaseDate = request.ReleaseDate;
        movie.Director = request.Director;
        movie.Casts = request.Casts;
        movie.PosterUrl = request.PosterUrl;
        movie.TrailerUrl = request.TrailerUrl;
        movie.AgeRestrictionId = request.AgeRestrictionId;

        // Clear existing genres and reconstruct
        movie.MovieGenres.Clear();
        if (request.GenreIds != null && request.GenreIds.Any())
        {
            foreach (var genreId in request.GenreIds)
            {
                movie.MovieGenres.Add(new MovieGenre { GenreId = genreId });
            }
        }

        _movieRepository.Update(movie);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var updatedMovie = await _movieRepository.GetByIdWithDetailsAsync(movie.Id, cancellationToken);
        return MovieDto.FromEntity(updatedMovie!);
    }
}
