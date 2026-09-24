using CinemaMS.Domain.Enums;

namespace CinemaMS.Application.Features.Articles.DTOs;

public class ArticleDto
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Summary { get; set; } = default!;
    public string Content { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public ArticleType Type { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
