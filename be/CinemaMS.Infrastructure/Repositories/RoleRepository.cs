using CinemaMS.Domain.Entities.Identity;
using CinemaMS.Domain.Repositories;
using CinemaMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CinemaMS.Infrastructure.Repositories;

public class RoleRepository : RepositoryBase<Role, int>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Role?> GetByNameAsync(string roleName, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Role>()
            .FirstOrDefaultAsync(x => x.Name == roleName, cancellationToken);
    }
}
