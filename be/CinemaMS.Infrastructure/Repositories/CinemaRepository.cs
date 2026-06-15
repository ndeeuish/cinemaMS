using CinemaMS.Domain.Entities.Cinemas;
using CinemaMS.Domain.Repositories;
using CinemaMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CinemaMS.Infrastructure.Repositories;

public class CinemaRepository : RepositoryBase<Cinema, int>, ICinemaRepository
{
    public CinemaRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(IEnumerable<Cinema> Items, int TotalCount)> GetPagedAsync(int pageIndex, int pageSize, string? keyword = null, CancellationToken cancellationToken = default)
    {
        var query = DbContext.Set<Cinema>().AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(x => x.Name.Contains(keyword) || x.Address.Contains(keyword));
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
