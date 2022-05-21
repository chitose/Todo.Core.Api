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