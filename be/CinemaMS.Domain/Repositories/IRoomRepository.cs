using CinemaMS.Domain.Entities.Cinemas;

namespace CinemaMS.Domain.Repositories;

public interface IRoomRepository : IRepository<Room>
{
    Task<bool> ExistsByNameInCinemaAsync(string name, int cinemaId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Room>> GetByCinemaIdAsync(int cinemaId, CancellationToken cancellationToken = default);
    Task<(IEnumerable<Room> Items, int TotalCount)> GetPagedByCinemaIdAsync(int cinemaId, int pageIndex, int pageSize, string? keyword = null, CancellationToken cancellationToken = default);
}
