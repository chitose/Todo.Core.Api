using NHibernate;
using NHibernate.Linq;
using Todo.Core.Common.UnitOfWork;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Repositories;

public abstract class GenericEntityRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
{
    protected ISession Session => UnitOfWork.Current.GetCurrentSession();

    public IQueryable<TEntity> GetAll()
    {
        return Session.Query<TEntity>();
    }

    public Task<TEntity> GetByKey(int key, CancellationToken cancellationToken = default)
    {
        return Session.Query<TEntity>()
            .Where(t => t.Id == key).SingleOrDefaultAsync(cancellationToken);
    }

    public Task Add(TEntity entity, CancellationToken cancellationToken = default)
    {
        return Session.SaveAsync(entity, cancellationToken);
    }

    public Task Save(TEntity entity, CancellationToken cancellationToken = default)
    {
        return Session.SaveOrUpdateAsync(entity, cancellationToken);
    }

    public Task Delete(TEntity entity, CancellationToken cancellationToken = default)
    {
        return Session.DeleteAsync(entity, cancellationToken);
    }

    public async Task DeleteByKey(int key, CancellationToken cancellationToken = default)
    {
        var entity = await GetByKey(key, cancellationToken);
        await Delete(entity, cancellationToken);
    }
}