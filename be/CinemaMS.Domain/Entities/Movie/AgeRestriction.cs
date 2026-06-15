using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CinemaMS.Domain.Common;

namespace CinemaMS.Domain.Entities.Catalog;

[Table("AgeRestrictions")]
public class AgeRestriction : EntityBase
{
    [Required]
    [MaxLength(10)]
    public string Code { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string Description { get; set; } = default!;

    public ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
