using ISession = NHibernate.ISession;

namespace Todo.Core.Common.UnitOfWork;

public class SessionAccessor : ISessionAccessor
{
    private readonly ISession _session;
    public SessionAccessor(ISession session)
    {
        _session = session;
    }

    public void Dispose()
    {
        _session.Dispose();
    }

    public IQueryable<TEntity> Query<TEntity>() where TEntity : class, new()
    {
        return _session.Query<TEntity>();
    }

    public Task SaveAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class, new()
    {
        return _session.SaveAsync(entity, cancellationToken);
    }

    public Task SaveOrUpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class, new()
    {
        return _session.SaveOrUpdateAsync(entity, cancellationToken);
    }

    public Task DeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class, new()
    {
        return _session.DeleteAsync(entity, cancellationToken);
    }

    public Task PersistAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class
    {
        return _session.PersistAsync(entity, cancellationToken);
    }
}