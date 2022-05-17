namespace Todo.Core.Common.UnitOfWork;

public interface IUnitOfWorkProvider
{
    IUnitOfWork Provide();

    IStatelessUnitOfWork ProvideStateless();

    Task<T> PerformActionInUnitOfWork<T>(Func<Task<T>> actionResult);
    
    Task PerformActionInUnitOfWork(Func<Task> actionResult);

    Task<T> PerformActionInUnitOfWorkStateless<T>(Func<Task<T>> actionResult);
    
    Task PerformActionInUnitOfWorkStateless(Func<Task> actionResult);
}