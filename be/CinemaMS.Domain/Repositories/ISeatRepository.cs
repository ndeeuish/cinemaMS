using CinemaMS.Domain.Entities.Cinemas;

namespace CinemaMS.Domain.Repositories;

public interface ISeatRepository : IRepository<Seat>
{
    Task<IEnumerable<Seat>> GetSeatsByRoomIdAsync(int roomId, CancellationToken cancellationToken = default);
    Task<bool> HasSeatsInRoomAsync(int roomId, CancellationToken cancellationToken = default);
}
