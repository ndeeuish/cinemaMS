using CinemaMS.Domain.Entities.Catalog;
using CinemaMS.Domain.Enums;
using CinemaMS.Domain.Repositories;
using CinemaMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CinemaMS.Infrastructure.Repositories;

public class ArticleRepository : RepositoryBase<Article, int>, IArticleRepository
{
    public ArticleRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(IEnumerable<Article> Items, int TotalCount)> GetPagedAsync(int pageIndex, int pageSize, string? keyword = null, ArticleType? type = null, bool? isActive = null, CancellationToken cancellationToken = default)
    {
        var query = DbContext.Set<Article>().AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(x => x.Title.Contains(keyword));
        }

        if (type.HasValue)
        {
            query = query.Where(x => x.Type == type.Value);
        }
        
        if (isActive.HasValue)
        {
            query = query.Where(x => x.IsActive == isActive.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);
        
        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
