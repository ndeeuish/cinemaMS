using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Repositories;

using CinemaMS.Domain.Exceptions;

namespace CinemaMS.Application.Features.Cinemas.Commands;

public class DeleteCinemaCommandHandler : CommandHandlerBase<DeleteCinemaCommand, int>
{
    private readonly ICinemaRepository _cinemaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCinemaCommandHandler(ICinemaRepository cinemaRepository, IUnitOfWork unitOfWork)
    {
        _cinemaRepository = cinemaRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<int> Handle(DeleteCinemaCommand request, CancellationToken cancellationToken)
    {
        var cinema = await _cinemaRepository.GetByIdAsync(request.Id, cancellationToken);
        if (cinema == null)
            throw new NotFoundException("Cinema", request.Id);

        _cinemaRepository.Delete(cinema);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return cinema.Id;
    }
}
