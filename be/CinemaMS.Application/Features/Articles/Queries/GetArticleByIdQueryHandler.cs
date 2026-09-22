using CinemaMS.Domain.Exceptions;
using CinemaMS.Application.Features.Articles.DTOs;
using CinemaMS.Domain.Entities.Catalog;
using CinemaMS.Domain.Repositories;
using MediatR;

namespace CinemaMS.Application.Features.Articles.Queries;

public class GetArticleByIdQueryHandler : IRequestHandler<GetArticleByIdQuery, ArticleDto>
{
    private readonly IArticleRepository _articleRepository;

    public GetArticleByIdQueryHandler(IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }

    public async Task<ArticleDto> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
    {
        var article = await _articleRepository.GetByIdAsync(request.Id, cancellationToken);

        if (article == null)
            throw new NotFoundException(nameof(Article), request.Id);

        return new ArticleDto
        {
            Id = article.Id,
            Title = article.Title,
            Summary = article.Summary,
            Content = article.Content,
            ImageUrl = article.ImageUrl,
            Type = article.Type,
            StartDate = article.StartDate,
            EndDate = article.EndDate,
            IsActive = article.IsActive,
            CreatedAt = article.CreatedAt
        };
    }
}
