using CinemaMS.Domain.Entities.Cinemas;

namespace CinemaMS.Application.Features.Seats.DTOs;

public class SeatDto
{
    public int Id { get; set; }
    public string RowIndex { get; set; } = default!;
    public int ColumnIndex { get; set; }
    public int SeatNumber { get; set; }
    public int RoomId { get; set; }
    public int SeatTypeId { get; set; }
    public string SeatTypeName { get; set; } = default!;
    public decimal PriceModifier { get; set; }
    
    public string Code => $"{RowIndex}{SeatNumber}";

    public static SeatDto FromEntity(Seat seat)
    {
        return new SeatDto
        {
            Id = seat.Id,
            RowIndex = seat.RowIndex,
            SeatNumber = seat.SeatNumber,
            ColumnIndex = seat.ColumnIndex,
            RoomId = seat.RoomId,
            SeatTypeId = seat.SeatTypeId,
            SeatTypeName = seat.SeatType?.Name ?? "Unknown",
            PriceModifier = seat.SeatType?.Surcharge ?? 0
        };
    }
}
