using CinemaMS.Application.Features.Articles.DTOs;
using CinemaMS.Application.Features.Common.Pagination;
using CinemaMS.Domain.Repositories;
using MediatR;

namespace CinemaMS.Application.Features.Articles.Queries;

public class GetAllArticlesQueryHandler : IRequestHandler<GetAllArticlesQuery, PagedResult<ArticleDto>>
{
    private readonly IArticleRepository _articleRepository;

    public GetAllArticlesQueryHandler(IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }

    public async Task<PagedResult<ArticleDto>> Handle(GetAllArticlesQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _articleRepository.GetPagedAsync(
            request.PageIndex, 
            request.PageSize, 
            request.Keyword,
            request.Type,
            request.IsActive,
            cancellationToken);

        var result = new PagedResult<ArticleDto>
        {
            Items = items.Select(a => new ArticleDto
            {
                Id = a.Id,
                Title = a.Title,
                Summary = a.Summary,
                Content = a.Content,
                ImageUrl = a.ImageUrl,
                Type = a.Type,
                StartDate = a.StartDate,
                EndDate = a.EndDate,
                IsActive = a.IsActive,
                CreatedAt = a.CreatedAt
            }),
            TotalItems = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };

        return result;
    }
}
