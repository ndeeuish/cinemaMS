using CinemaMS.Domain.Entities.Identity;

namespace CinemaMS.Domain.Repositories;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role?> GetByNameAsync(string roleName, CancellationToken cancellationToken = default);
}
