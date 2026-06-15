using CinemaMS.Application.Messaging;

namespace CinemaMS.Application.Features.Seats.Commands;

public class CreateBulkSeatsCommand : CommandBase<bool>
{
    public int RoomId { get; set; }
    
    // Matrix n x m
    // 0 = No seat
    // 1, 2, 3... = SeatTypeId
    public int[][] Matrix { get; set; } = Array.Empty<int[]>();
}
