using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CinemaMS.Domain.Common;

namespace CinemaMS.Domain.Entities.Cinemas;

[Table("RoomTypes")]
public class RoomType : EntityBase
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = default!;
    public decimal Surcharge { get; set; }
}
