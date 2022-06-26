using NHibernate;
using Todo.Core.Common.Configuration;
using Todo.Core.Common.Extensions;

namespace Todo.Core.Common.UnitOfWork;

public class UnitOfWorkProvider : IUnitOfWorkProvider
{
    private readonly IEnumerable<ISessionListener> _listeners;
    private readonly ISessionFactory _sessionFactory;

    public UnitOfWorkProvider(ISessionFactory sessionFactory,
        IConfigProvider configProvider,
        IEnumerable<ISessionListener> listeners)
    {
        _sessionFactory = sessionFactory;
        var connectionType = configProvider.GetConnectionType();
        _listeners = listeners.Where(l => string.IsNullOrWhiteSpace(l.ConnectionType)
        || l.ConnectionType == connectionType);
    }

    public IUnitOfWork Provide()
    {
        return new UnitOfWork(_sessionFactory, _listeners);
    }

    public IStatelessUnitOfWork ProvideStateless()
    {
        return new StatelessUnitOfWork(_sessionFactory, _listeners);
    }

    public T PerformActionInUnitOfWork<T>(Func<T> actionResult)
    {
        using var uow = Provide();
        var result = actionResult();
        uow.Commit();
        return result;
    }

    public void PerformActionInUnitOfWork(Action actionResult)
    {
        using var uow = Provide();
        actionResult();
        uow.Commit();
    }

    public async Task<T> PerformActionInUnitOfWork<T>(Func<Task<T>> actionResult)
    {
        await using var uow = Provide();
        var result = await actionResult();
        await uow.CommitAsync();
        return result;
    }

    public async Task PerformActionInUnitOfWork(Func<Task> actionResult)
    {
        await using var uow = Provide();
        await actionResult();
        await uow.CommitAsync();
    }

    public async Task<T> PerformActionInUnitOfWorkStateless<T>(Func<Task<T>> actionResult)
    {
        await using var uow = ProvideStateless();
        return await actionResult();
    }

    public async Task PerformActionInUnitOfWorkStateless(Func<Task> actionResult)
    {
        await using var uow = ProvideStateless();
        await actionResult();
    }
}