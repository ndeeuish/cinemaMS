using CinemaMS.Application.Features.Articles.DTOs;
using CinemaMS.Application.Features.Common.Pagination;
using CinemaMS.Domain.Enums;
using MediatR;

namespace CinemaMS.Application.Features.Articles.Queries;

public class GetAllArticlesQuery : FilterBase, IRequest<PagedResult<ArticleDto>>
{
    public ArticleType? Type { get; set; }
    public bool? IsActive { get; set; }
}
