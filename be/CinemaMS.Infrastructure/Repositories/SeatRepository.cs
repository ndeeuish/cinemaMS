using CinemaMS.Domain.Entities.Cinemas;
using CinemaMS.Domain.Repositories;
using CinemaMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CinemaMS.Infrastructure.Repositories;

public class SeatRepository : RepositoryBase<Seat, int>, ISeatRepository
{
    public SeatRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Seat>> GetSeatsByRoomIdAsync(int roomId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Seat>()
            .AsNoTracking()
            .Include(s => s.SeatType)
            .Where(s => s.RoomId == roomId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasSeatsInRoomAsync(int roomId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Seat>()
            .AnyAsync(s => s.RoomId == roomId, cancellationToken);
    }
}
