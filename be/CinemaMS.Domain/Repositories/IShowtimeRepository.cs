using CinemaMS.Domain.Entities.Bookings;

namespace CinemaMS.Domain.Repositories;

public interface IShowtimeRepository : IRepository<Showtime>
{
    Task<Showtime?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Showtime>> GetByMovieIdAsync(int movieId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Showtime>> GetByCinemaIdAsync(int cinemaId, CancellationToken cancellationToken = default);
    Task<(IEnumerable<Showtime> Items, int TotalCount)> GetPagedByCinemaIdAsync(int cinemaId, int pageIndex, int pageSize, string? keyword = null, CancellationToken cancellationToken = default);
    Task<bool> HasOverlappingShowtimeAsync(int roomId, DateTime startTime, DateTime endTime, int? excludeShowtimeId = null, CancellationToken cancellationToken = default);
}
