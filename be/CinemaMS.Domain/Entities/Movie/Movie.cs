using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CinemaMS.Domain.Common;

namespace CinemaMS.Domain.Entities.Catalog;

[Table("Movies")]
public class Movie : EntityBase
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = default!;

    [MaxLength(2000)]
    public string Description { get; set; } = default!;
    public int DurationInMinutes { get; set; }
    public DateTime ReleaseDate { get; set; }
    
    [MaxLength(100)]
    public string Director { get; set; } = default!;
    
    [MaxLength(1000)]
    public string Casts { get; set; } = default!;
    
    public string PosterUrl { get; set; } = default!;
    public string TrailerUrl { get; set; } = default!;

    // Fk
    public int AgeRestrictionId { get; set; }
    [ForeignKey(nameof(AgeRestrictionId))]
    public AgeRestriction AgeRestriction { get; set; } = default!;

    public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
}
