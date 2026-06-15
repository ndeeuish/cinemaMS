using CinemaMS.Domain.Entities.Cinemas;
using CinemaMS.Domain.Repositories;
using CinemaMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CinemaMS.Infrastructure.Repositories;

public class RoomRepository : RepositoryBase<Room, int>, IRoomRepository
{
    public RoomRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> ExistsByNameInCinemaAsync(string name, int cinemaId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Room>()
            .AnyAsync(r => r.Name == name && r.CinemaId == cinemaId, cancellationToken);
    }

    public async Task<IEnumerable<Room>> GetByCinemaIdAsync(int cinemaId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Room>()
            .AsNoTracking()
            .Include(r => r.RoomType)
            .Where(r => r.CinemaId == cinemaId)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<Room> Items, int TotalCount)> GetPagedByCinemaIdAsync(int cinemaId, int pageIndex, int pageSize, string? keyword = null, CancellationToken cancellationToken = default)
    {
        var query = DbContext.Set<Room>()
            .AsNoTracking()
            .Include(r => r.RoomType)
            .Where(r => r.CinemaId == cinemaId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(x => x.Name.Contains(keyword));
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
