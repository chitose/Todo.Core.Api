using NHibernate;

namespace Todo.Core.Common.UnitOfWork;

public class UnitOfWorkProvider : IUnitOfWorkProvider
{
    private readonly ISessionFactory _sessionFactory;
    public UnitOfWorkProvider(ISessionFactory sessionFactory)
    {
        _sessionFactory = sessionFactory;
    }
    
    public IUnitOfWork Provide()
    {
        return new UnitOfWork(_sessionFactory);
    }

    public IStatelessUnitOfWork ProvideStateless()
    {
        return new StatelessUnitOfWork(_sessionFactory);
    }
}