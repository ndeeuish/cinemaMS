using CinemaMS.Domain.Entities.Catalog;

namespace CinemaMS.Domain.Repositories;

public interface IMovieRepository : IRepository<Movie>
{
    Task<IEnumerable<Movie>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);
    Task<(IEnumerable<Movie> Items, int TotalCount)> GetPagedWithDetailsAsync(int pageIndex, int pageSize, string? keyword = null, int? genreId = null, CancellationToken cancellationToken = default);
    Task<Movie?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
}
