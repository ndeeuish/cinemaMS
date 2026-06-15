using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CinemaMS.Domain.Common;

namespace CinemaMS.Domain.Entities.Catalog;

[Table("Genres")]
public class Genre : EntityBase
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = default!;

    public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
}
