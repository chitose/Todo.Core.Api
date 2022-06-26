using NHibernate;

namespace Todo.Core.Common.UnitOfWork;

public class StatelessUnitOfWork : BaseUnitOfWork<IStatelessUnitOfWork, IStatelessSession>, IStatelessUnitOfWork
{
    public StatelessUnitOfWork(ISessionFactory sessionFactory, IEnumerable<ISessionListener> listeners)
    {
        LazySession = new Lazy<IStatelessSession>(() =>
        {
            var session = sessionFactory.OpenStatelessSession();
            foreach (var l in listeners) l.OnOpen(new StatelessSessionAccessor(session));
            return session;
        });
    }

    public override ISessionAccessor GetCurrentSession()
    {
        return new StatelessSessionAccessor(LazySession.Value);
    }
}