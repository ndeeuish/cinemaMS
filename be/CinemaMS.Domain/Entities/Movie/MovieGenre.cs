using System.ComponentModel.DataAnnotations.Schema;
using CinemaMS.Domain.Common;

namespace CinemaMS.Domain.Entities.Catalog;

[Table("MovieGenres")]
public class MovieGenre : EntityBase
{
    public int MovieId { get; set; }
    public int GenreId { get; set; }

    // Fk
    [ForeignKey(nameof(MovieId))]
    public Movie Movie { get; set; } = default!;

    [ForeignKey(nameof(GenreId))]
    public Genre Genre { get; set; } = default!;
}
