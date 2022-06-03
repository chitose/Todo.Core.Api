using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Repositories;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity, new()
{
    IQueryable<TEntity> GetAll();

    Task<TEntity?> GetByKey(int key, CancellationToken cancellationToken = default);

    Task<TEntity> Add(TEntity entity, CancellationToken cancellationToken = default);

    Task<TEntity> Save(TEntity entity, CancellationToken cancellationToken = default);

    Task Delete(TEntity? entity, CancellationToken cancellationToken = default);

    Task DeleteByKey(int key, CancellationToken cancellationToken = default);
}