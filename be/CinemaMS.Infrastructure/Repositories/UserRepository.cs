using CinemaMS.Domain.Entities.Identity;
using CinemaMS.Domain.Repositories;
using CinemaMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CinemaMS.Infrastructure.Repositories;

public class UserRepository : RepositoryBase<User, int>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<User>().AnyAsync(x => x.Email == email, cancellationToken);
    }

    public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<User>().AnyAsync(x => x.Username == username, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<User>()
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<User>()
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<User>()
            .AsNoTracking()
            .Include(x => x.Role)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<User> Items, int TotalCount)> GetPagedWithDetailsAsync(int pageIndex, int pageSize, int? roleId = null, string? keyword = null, CancellationToken cancellationToken = default)
    {
        var query = DbContext.Set<User>().AsNoTracking().Include(x => x.Role).AsQueryable();

        if (roleId.HasValue)
        {
            query = query.Where(x => x.RoleId == roleId.Value);
        }

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(x => x.Username.Contains(keyword) || x.Email.Contains(keyword) || x.FullName.Contains(keyword));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        
        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<User?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<User>()
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}
