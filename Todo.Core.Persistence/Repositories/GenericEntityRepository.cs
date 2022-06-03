using NHibernate;
using NHibernate.Linq;
using Todo.Core.Common.UnitOfWork;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Repositories;

public abstract class GenericEntityRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
{
    protected ISession Session => UnitOfWork.Current?.GetCurrentSession();

    public virtual IQueryable<TEntity> GetAll()
    {
        return Session.Query<TEntity>();
    }

    public virtual Task<TEntity?> GetByKey(int key, CancellationToken cancellationToken = default)
    {
        return Session.Query<TEntity>()
            .Where(t => t.Id == key).SingleOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TEntity> Add(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Session.SaveAsync(entity, cancellationToken);
        return entity;
    }

    public virtual async Task<TEntity> Save(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Session.SaveOrUpdateAsync(entity, cancellationToken);
        return entity;
    }

    public virtual Task Delete(TEntity? entity, CancellationToken cancellationToken = default)
    {
        return Session.DeleteAsync(entity, cancellationToken);
    }

    public virtual async Task DeleteByKey(int key, CancellationToken cancellationToken = default)
    {
        var entity = await GetByKey(key, cancellationToken);
        await Delete(entity, cancellationToken);
    }
}