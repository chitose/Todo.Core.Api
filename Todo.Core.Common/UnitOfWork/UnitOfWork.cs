using NHibernate;
using ISession = NHibernate.ISession;

namespace Todo.Core.Common.UnitOfWork;

public class UnitOfWork : BaseUnitOfWork<IUnitOfWork, ISession>, IUnitOfWork
{
    private ITransaction? _transaction;

    public UnitOfWork(ISessionFactory sessionFactory)
    {
        _lazySession = new Lazy<ISession>(() =>
        {
            var session = sessionFactory.WithOptions().OpenSession();
            _transaction = session.BeginTransaction();
            return session;
        });
    }

    public async Task CommitAsync()
    {
        try
        {
            await _transaction?.CommitAsync()!;
        }
        catch
        {
            await _transaction?.RollbackAsync()!;
            throw;
        }
    }

    public void Commit()
    {
        try
        {
            _transaction?.Commit();
        }
        catch
        {
            _transaction?.Rollback();
            throw;
        }
    }
}