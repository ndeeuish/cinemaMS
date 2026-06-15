using CinemaMS.Application.Features.Seats.Commands;
using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Entities.Cinemas;
using CinemaMS.Domain.Exceptions;
using CinemaMS.Domain.Repositories;

namespace CinemaMS.Application.Features.Seats.Commands;

public class CreateBulkSeatsCommandHandler : CommandHandlerBase<CreateBulkSeatsCommand, bool>
{
    private readonly ISeatRepository _seatRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBulkSeatsCommandHandler(
        ISeatRepository seatRepository,
        IRoomRepository roomRepository,
        IUnitOfWork unitOfWork)
    {
        _seatRepository = seatRepository;
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<bool> Handle(CreateBulkSeatsCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.GetByIdAsync(request.RoomId, cancellationToken);
        if (room == null)
            throw new NotFoundException(nameof(Room), request.RoomId);

        // Delete existing seats
        var existingSeats = await _seatRepository.GetSeatsByRoomIdAsync(request.RoomId, cancellationToken);
        foreach (var existingSeat in existingSeats)
        {
            _seatRepository.Delete(existingSeat);
        }

        var seatsToCreate = new List<Seat>();
        
        for (int i = 0; i < request.Matrix.Length; i++)
        {
            char rowIndex = (char)('A' + i);
            int[] rowConfig = request.Matrix[i];
            
            int physicalSeatCounter = 1;
            
            for (int j = 0; j < rowConfig.Length; j++)
            {
                int seatTypeId = rowConfig[j];
                
                // If 0, meaning 'No Seat', skip space.
                if (seatTypeId > 0)
                {
                    seatsToCreate.Add(new Seat
                    {
                        RoomId = request.RoomId,
                        RowIndex = rowIndex.ToString(),
                        ColumnIndex = j,
                        SeatNumber = physicalSeatCounter++,
                        SeatTypeId = seatTypeId
                    });
                }
            }
        }
        
        if (seatsToCreate.Count > room.Capacity)
        {
            throw new UserFriendlyException($"Số lượng ghế tạo ra ({seatsToCreate.Count}) vượt quá sức chứa của phòng ({room.Capacity}). Vui lòng tăng sức chứa của phòng trước.");
        }

        // remove the room.Capacity = seatsToCreate.Count update so capacity remains fixed.
        // _roomRepository.Update(room); // no longer needed since we don't update room

        foreach (var seat in seatsToCreate)
        {
            _seatRepository.Add(seat);
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
