using NHibernate;

namespace Todo.Core.Common.UnitOfWork;

public class StatelessSessionAccessor : ISessionAccessor
{
    private readonly IStatelessSession _statelessSession;
    public StatelessSessionAccessor(IStatelessSession session)
    {
        _statelessSession = session;
    }
    
    public void Dispose()
    {
        _statelessSession.Dispose();
    }

    public IQueryable<TEntity> Query<TEntity>() where TEntity : class, new()
    {
        return _statelessSession.Query<TEntity>();
    }

    public Task SaveAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class, new()
    {
        return _statelessSession.InsertAsync(entity, cancellationToken);
    }

    public Task SaveOrUpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class, new()
    {
        return _statelessSession.UpdateAsync(entity, cancellationToken);
    }

    public Task DeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class, new()
    {
        return _statelessSession.DeleteAsync(entity, cancellationToken);
    }

    public Task PersistAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class
    {
        return _statelessSession.InsertAsync(entity, cancellationToken);
    }
}