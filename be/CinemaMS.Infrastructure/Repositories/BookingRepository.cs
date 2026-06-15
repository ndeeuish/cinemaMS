using CinemaMS.Domain.Entities.Bookings;
using CinemaMS.Domain.Repositories;
using CinemaMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CinemaMS.Infrastructure.Repositories;

public class BookingRepository : RepositoryBase<Booking, int>, IBookingRepository
{
    public BookingRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<int>> GetReservedSeatIdsAsync(int showtimeId, CancellationToken cancellationToken = default)
    {
        var utcNow = DateTime.UtcNow;

        return await DbContext.Set<Ticket>()
            .Include(t => t.Booking)
            .Where(t => t.ShowtimeId == showtimeId &&
                        (t.Booking.Status == "Confirmed" || 
                        (t.Booking.Status == "Holding" && t.Booking.HoldExpiration > utcNow)))
            .Select(t => t.SeatId)
            .Distinct()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Booking>> GetUserBookingsAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Booking>()
            .AsNoTracking()
            .Include(b => b.Tickets)
                .ThenInclude(t => t.Showtime)
                    .ThenInclude(s => s.Movie)
            .Include(b => b.Tickets)
                .ThenInclude(t => t.Showtime)
                    .ThenInclude(s => s.Room)
                        .ThenInclude(r => r.Cinema)
            .Include(b => b.Tickets)
                .ThenInclude(t => t.Seat)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Booking?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Booking>()
            .Include(b => b.Tickets)
                .ThenInclude(t => t.Showtime)
                    .ThenInclude(s => s.Movie)
            .Include(b => b.Tickets)
                .ThenInclude(t => t.Showtime)
                    .ThenInclude(s => s.Room)
                        .ThenInclude(r => r.Cinema)
            .Include(b => b.Tickets)
                .ThenInclude(t => t.Seat)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<(IEnumerable<Booking> Items, int TotalCount)> GetPagedAsync(int pageIndex, int pageSize, string? keyword = null, CancellationToken cancellationToken = default)
    {
        var query = DbContext.Set<Booking>()
            .AsNoTracking()
            .Include(b => b.User)
            .Include(b => b.Tickets)
                .ThenInclude(t => t.Showtime)
                    .ThenInclude(s => s.Movie)
            .Include(b => b.Tickets)
                .ThenInclude(t => t.Showtime)
                    .ThenInclude(s => s.Room)
                        .ThenInclude(r => r.Cinema)
            .Include(b => b.Tickets)
                .ThenInclude(t => t.Seat)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(b => 
                (b.User != null && b.User.FullName.Contains(keyword)) ||
                (b.User != null && b.User.Email.Contains(keyword)) ||
                b.Id.ToString() == keyword);
        }

        var totalCount = await query.CountAsync(cancellationToken);
        
        var items = await query
            .OrderByDescending(b => b.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
