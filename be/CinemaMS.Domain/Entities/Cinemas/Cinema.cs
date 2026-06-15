using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CinemaMS.Domain.Common;

namespace CinemaMS.Domain.Entities.Cinemas;

[Table("Cinemas")]
public class Cinema : EntityBase
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = default!;

    [Required]
    [MaxLength(255)]
    public string Address { get; set; } = default!;

    [MaxLength(20)]
    public string Hotline { get; set; } = default!;
}
