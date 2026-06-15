using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Entities.Catalog;
using CinemaMS.Domain.Exceptions;
using CinemaMS.Domain.Repositories;

namespace CinemaMS.Application.Features.Movies.Commands;

public class DeleteMovieCommandHandler : CommandHandlerBase<DeleteMovieCommand, int>
{
    private readonly IMovieRepository _movieRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMovieCommandHandler(IMovieRepository movieRepository, IUnitOfWork unitOfWork)
    {
        _movieRepository = movieRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<int> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdAsync(request.Id, cancellationToken);
        if (movie == null)
            throw new NotFoundException(nameof(Movie), request.Id);

        _movieRepository.Delete(movie);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return movie.Id;
    }
}
