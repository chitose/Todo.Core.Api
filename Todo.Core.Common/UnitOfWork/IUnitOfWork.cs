using ISession = NHibernate.ISession;

namespace Todo.Core.Common.UnitOfWork;

public interface IUnitOfWork : IBaseUnitOfWork<ISession>
{
    Task CommitAsync();

    void Commit();
}