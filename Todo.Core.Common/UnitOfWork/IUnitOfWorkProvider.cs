namespace Todo.Core.Common.UnitOfWork;

public interface IUnitOfWorkProvider
{
    IUnitOfWork Provide();

    IStatelessUnitOfWork ProvideStateless();

    T PerformActionInUnitOfWork<T>(Func<T> actionResult);
    
    void PerformActionInUnitOfWork(Action actionResult);
    
    Task<T> PerformActionInUnitOfWork<T>(Func<Task<T>> actionResult);
    
    Task PerformActionInUnitOfWork(Func<Task> actionResult);

    Task<T> PerformActionInUnitOfWorkStateless<T>(Func<Task<T>> actionResult);
    
    Task PerformActionInUnitOfWorkStateless(Func<Task> actionResult);
}