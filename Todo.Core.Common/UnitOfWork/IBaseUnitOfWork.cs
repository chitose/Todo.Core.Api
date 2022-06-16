namespace Todo.Core.Common.UnitOfWork;

public interface IBaseUnitOfWork<out TSession> : IDisposable, IAsyncDisposable
{
    IBaseUnitOfWork<TSession>? Parent { get; }
    ISessionAccessor GetCurrentSession();
}