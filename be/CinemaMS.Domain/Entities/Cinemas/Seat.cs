using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CinemaMS.Domain.Common;

namespace CinemaMS.Domain.Entities.Cinemas;

[Table("Seats")]
public class Seat : EntityBase
{
    [Required]
    [MaxLength(10)]
    public string RowIndex { get; set; } = default!;
    public int ColumnIndex { get; set; }
    public int SeatNumber { get; set; }
    public int RoomId { get; set; }
    public int SeatTypeId { get; set; }

    // Fk
    [ForeignKey(nameof(RoomId))]
    public Room Room { get; set; } = default!;

    [ForeignKey(nameof(SeatTypeId))]
    public SeatType SeatType { get; set; } = default!;
}
