using ISession = NHibernate.ISession;

namespace Todo.Core.Common.UnitOfWork;

public interface IBaseUnitOfWork<out TSession> : IDisposable, IAsyncDisposable
{
    TSession GetCurrentSession();

    IBaseUnitOfWork<TSession>? Parent { get; }
}