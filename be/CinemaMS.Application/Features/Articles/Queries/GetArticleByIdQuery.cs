using CinemaMS.Application.Features.Articles.DTOs;
using MediatR;

namespace CinemaMS.Application.Features.Articles.Queries;

public class GetArticleByIdQuery : IRequest<ArticleDto>
{
    public int Id { get; set; }

    public GetArticleByIdQuery(int id)
    {
        Id = id;
    }
}
