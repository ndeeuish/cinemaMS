using CinemaMS.Domain.Entities.Catalog;
using CinemaMS.Domain.Repositories;
using CinemaMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CinemaMS.Infrastructure.Repositories;

public class MovieRepository : RepositoryBase<Movie, int>, IMovieRepository
{
    public MovieRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Movie>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Movie>()
            .AsNoTracking()
            .Include(m => m.AgeRestriction)
            .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<Movie> Items, int TotalCount)> GetPagedWithDetailsAsync(int pageIndex, int pageSize, string? keyword = null, int? genreId = null, CancellationToken cancellationToken = default)
    {
        var query = DbContext.Set<Movie>()
            .AsNoTracking()
            .Include(m => m.AgeRestriction)
            .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(x => x.Title.Contains(keyword));
        }

        if (genreId.HasValue)
        {
            query = query.Where(x => x.MovieGenres.Any(mg => mg.GenreId == genreId.Value));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        
        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<Movie?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Movie>()
            .Include(m => m.AgeRestriction)
            .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }
}
