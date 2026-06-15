using CinemaMS.Domain.Entities.Identity;

namespace CinemaMS.Domain.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);
    Task<(IEnumerable<User> Items, int TotalCount)> GetPagedWithDetailsAsync(int pageIndex, int pageSize, int? roleId = null, string? keyword = null, CancellationToken cancellationToken = default);
    Task<User?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
}
