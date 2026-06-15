using System.Linq.Expressions;
using CinemaMS.Domain.Common;

namespace CinemaMS.Domain.Repositories;

public interface IRepository<TEntity, TId> where TEntity : EntityBase<TId>
{
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}

public interface IRepository<TEntity> : IRepository<TEntity, int> where TEntity : EntityBase<int>
{
}
