using CinemaMS.Domain.Entities.Cinemas;

namespace CinemaMS.Domain.Repositories;

public interface ICinemaRepository : IRepository<Cinema>
{
    Task<(IEnumerable<Cinema> Items, int TotalCount)> GetPagedAsync(int pageIndex, int pageSize, string? keyword = null, CancellationToken cancellationToken = default);
}
