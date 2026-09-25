using CinemaMS.Domain.Entities.Bookings;
using CinemaMS.Domain.Repositories;
using CinemaMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CinemaMS.Infrastructure.Repositories;

public class ShowtimeRepository : RepositoryBase<Showtime, int>, IShowtimeRepository
{
    public ShowtimeRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Showtime?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Showtime>()
            .Include(s => s.Movie)
            .Include(s => s.Room)
                .ThenInclude(r => r.Cinema)
            .Include(s => s.Room)
                .ThenInclude(r => r.RoomType)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Showtime>> GetByCinemaIdAsync(int cinemaId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Showtime>()
            .AsNoTracking()
            .Include(s => s.Movie)
            .Include(s => s.Room)
                .ThenInclude(r => r.Cinema)
            .Where(s => s.Room.CinemaId == cinemaId)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<Showtime> Items, int TotalCount)> GetPagedByCinemaIdAsync(int cinemaId, int pageIndex, int pageSize, string? keyword = null, CancellationToken cancellationToken = default)
    {
        var query = DbContext.Set<Showtime>()
            .AsNoTracking()
            .Include(s => s.Movie)
            .Include(s => s.Room)
                .ThenInclude(r => r.Cinema)
            .Where(s => s.Room.CinemaId == cinemaId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(x => x.Movie.Title.Contains(keyword) || x.Room.Name.Contains(keyword));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        
        var items = await query
            .OrderByDescending(x => x.StartTime)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<IEnumerable<Showtime>> GetByMovieIdAsync(int movieId, CancellationToken cancellationToken = default)
    {
        var nowUtc = DateTime.UtcNow;
        
        return await DbContext.Set<Showtime>()
            .AsNoTracking()
            .Include(s => s.Room)
                .ThenInclude(r => r.Cinema)
            .Where(s => s.MovieId == movieId && s.StartTime >= nowUtc)
            .OrderBy(s => s.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasOverlappingShowtimeAsync(int roomId, DateTime startTime, DateTime endTime, int? excludeShowtimeId = null, CancellationToken cancellationToken = default)
    {
        var query = DbContext.Set<Showtime>().Where(s => s.RoomId == roomId);

        if (excludeShowtimeId.HasValue)
        {
            query = query.Where(s => s.Id != excludeShowtimeId.Value);
        }

        // Overlap occurs if NewStart < ExistingEnd AND NewEnd > ExistingStart
        return await query.AnyAsync(s => startTime < s.EndTime && endTime > s.StartTime, cancellationToken);
    }
}
