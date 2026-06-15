using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Entities.Bookings;
using CinemaMS.Domain.Exceptions;
using CinemaMS.Domain.Repositories;

namespace CinemaMS.Application.Features.Showtimes.Commands;

public class DeleteShowtimeCommandHandler : CommandHandlerBase<DeleteShowtimeCommand, int>
{
    private readonly IShowtimeRepository _showtimeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteShowtimeCommandHandler(IShowtimeRepository showtimeRepository, IUnitOfWork unitOfWork)
    {
        _showtimeRepository = showtimeRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<int> Handle(DeleteShowtimeCommand request, CancellationToken cancellationToken)
    {
        var showtime = await _showtimeRepository.GetByIdAsync(request.Id, cancellationToken);
        if (showtime == null) throw new NotFoundException(nameof(Showtime), request.Id);

        // delete skip check have booking
        _showtimeRepository.Delete(showtime);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return showtime.Id;
    }
}
