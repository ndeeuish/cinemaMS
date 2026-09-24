using CinemaMS.Application.Features.Articles.DTOs;
using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Enums;
using System.Text.Json.Serialization;

namespace CinemaMS.Application.Features.Articles.Commands;

public class UpdateArticleCommand : CommandBase<ArticleDto>
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Summary { get; set; } = default!;
    public string Content { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public ArticleType Type { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
}
