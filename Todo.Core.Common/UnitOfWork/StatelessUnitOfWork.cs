using NHibernate;

namespace Todo.Core.Common.UnitOfWork;

public class StatelessUnitOfWork : BaseUnitOfWork<IStatelessUnitOfWork, IStatelessSession>, IStatelessUnitOfWork
{
    public StatelessUnitOfWork(ISessionFactory sessionFactory)
    {
        _lazySession = new Lazy<IStatelessSession>(sessionFactory.OpenStatelessSession);
    }
}