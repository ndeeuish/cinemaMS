using CinemaMS.Application.Features.Movies.DTOs;
using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Entities.Catalog;
using CinemaMS.Domain.Exceptions;
using CinemaMS.Domain.Repositories;

namespace CinemaMS.Application.Features.Movies.Queries;

public class GetMovieByIdQueryHandler : QueryHandlerBase<GetMovieByIdQuery, MovieDto>
{
    private readonly IMovieRepository _movieRepository;

    public GetMovieByIdQueryHandler(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public override async Task<MovieDto> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);
        if (movie == null)
            throw new NotFoundException(nameof(Movie), request.Id);

        return MovieDto.FromEntity(movie);
    }
}
