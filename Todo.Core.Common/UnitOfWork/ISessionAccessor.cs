using NHibernate;

namespace Todo.Core.Common.UnitOfWork;

public interface ISessionAccessor : IDisposable
{
    IQueryable<TEntity> Query<TEntity>() where TEntity : class, new();
    Task SaveAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class, new();
    Task SaveOrUpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class, new();
    Task DeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class, new();
    Task PersistAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class;
    IQueryOver<TEntity, TEntity> QueryOver<TEntity>() where TEntity : class, new();
    ISQLQuery CreateSql(string sqlStatement);
}