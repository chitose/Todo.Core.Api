using NHibernate;

namespace Todo.Core.Common.UnitOfWork;

public class StatelessUnitOfWork : BaseUnitOfWork<IStatelessUnitOfWork, IStatelessSession>, IStatelessUnitOfWork
{
    public StatelessUnitOfWork(ISessionFactory sessionFactory)
    {
        LazySession = new Lazy<IStatelessSession>(sessionFactory.OpenStatelessSession);
    }

    public override ISessionAccessor GetCurrentSession()
    {
        return new StatelessSessionAccessor(LazySession.Value);
    }
}