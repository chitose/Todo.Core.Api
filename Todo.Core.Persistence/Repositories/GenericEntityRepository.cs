using NHibernate;
using NHibernate.Linq;
using Todo.Core.Common.UnitOfWork;
using Todo.Core.Persistence.Entities;
using Todo.Core.Persistence.Exceptions;

namespace Todo.Core.Persistence.Repositories;

public abstract class GenericEntityRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
{
    protected ISessionAccessor Session =>
        UnitOfWork.Current?.GetCurrentSession() ?? StatelessUnitOfWork.Current?.GetCurrentSession();

    public virtual IQueryable<TEntity> GetQuery()
    {
        return Session.Query<TEntity>();
    }

    public virtual IQueryOver<TEntity, TEntity> GetQueryOver()
    {
        return Session.QueryOver<TEntity>();
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
        if (entity == null) throw new EntityNotFoundException(key, GetType().Name);
        await Delete(entity, cancellationToken);
    }
}