using CinemaMS.Domain.Entities.Bookings;

namespace CinemaMS.Domain.Repositories;

public interface IBookingRepository : IRepository<Booking>
{
    Task<IEnumerable<int>> GetReservedSeatIdsAsync(int showtimeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetUserBookingsAsync(int userId, CancellationToken cancellationToken = default);
    Task<Booking?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<(IEnumerable<Booking> Items, int TotalCount)> GetPagedAsync(int pageIndex, int pageSize, string? keyword = null, CancellationToken cancellationToken = default);
}
