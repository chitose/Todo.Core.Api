using NHibernate;
using ISession = NHibernate.ISession;

namespace Todo.Core.Common.UnitOfWork;

public abstract class BaseUnitOfWork<TUnit,TSession> : IBaseUnitOfWork<TSession> where TUnit : IBaseUnitOfWork<TSession> where TSession : IDisposable
{
    protected Lazy<TSession> _lazySession;

    private static IBaseUnitOfWork<TSession> _current;

    private static IBaseUnitOfWork<TSession> _parent;

    public static IBaseUnitOfWork<TSession> Current => _current;

    protected BaseUnitOfWork()
    {
        _parent = _current;
        _current = this;
    }

    public TSession GetCurrentSession()
    {
        return _lazySession.Value;
    }

    protected virtual void OnDispose()
    {
        // to be override by sub classes
    }
    public void Dispose()
    {
        _current = _parent;
        GetCurrentSession()?.Dispose();
        OnDispose();
    }

    public ValueTask DisposeAsync()
    {
        Dispose();
        return new ValueTask(Task.CompletedTask);
    }
}