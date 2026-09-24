using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CinemaMS.Domain.Common;
using CinemaMS.Domain.Enums;

namespace CinemaMS.Domain.Entities.Catalog;

[Table("Articles")]
public class Article : EntityBase
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = default!;

    [MaxLength(500)]
    public string Summary { get; set; } = default!;

    public string Content { get; set; } = default!;

    public string ImageUrl { get; set; } = default!;

    public ArticleType Type { get; set; }

    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }

    public bool IsActive { get; set; } = true;
}
