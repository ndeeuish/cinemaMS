using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CinemaMS.Domain.Common;

namespace CinemaMS.Domain.Entities.Cinemas;

[Table("Rooms")]
public class Room : EntityBase
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = default!;
    public int Capacity { get; set; }
    public int CinemaId { get; set; }
    public int RoomTypeId { get; set; }

    // Fk
    [ForeignKey(nameof(RoomTypeId))]
    public RoomType RoomType { get; set; } = default!;

    [ForeignKey(nameof(CinemaId))]
    public Cinema Cinema { get; set; } = default!;

    public ICollection<Seat> Seats { get; set; } = new List<Seat>();
}
