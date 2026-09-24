using CinemaMS.Domain.Entities.Catalog;
using CinemaMS.Domain.Enums;

namespace CinemaMS.Domain.Repositories;

public interface IArticleRepository : IRepository<Article>
{
    Task<(IEnumerable<Article> Items, int TotalCount)> GetPagedAsync(int pageIndex, int pageSize, string? keyword = null, ArticleType? type = null, bool? isActive = null, CancellationToken cancellationToken = default);
}
