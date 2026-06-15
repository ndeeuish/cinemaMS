using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Entities.Cinemas;
using CinemaMS.Domain.Exceptions;
using CinemaMS.Domain.Repositories;

namespace CinemaMS.Application.Features.Rooms.Commands;

public class DeleteRoomCommandHandler : CommandHandlerBase<DeleteRoomCommand, int>
{
    private readonly IRoomRepository _roomRepository;
    private readonly ISeatRepository _seatRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRoomCommandHandler(
        IRoomRepository roomRepository,
        ISeatRepository seatRepository,
        IUnitOfWork unitOfWork)
    {
        _roomRepository = roomRepository;
        _seatRepository = seatRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<int> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.GetByIdAsync(request.Id, cancellationToken);
        if (room == null)
            throw new NotFoundException(nameof(Room), request.Id);

        var existingSeats = (await _seatRepository.GetSeatsByRoomIdAsync(request.Id, cancellationToken)).ToList();
        if (existingSeats.Any())
        {
            foreach (var seat in existingSeats)
            {
                _seatRepository.Delete(seat);
            }
        }

        _roomRepository.Delete(room);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return room.Id;
    }
}
