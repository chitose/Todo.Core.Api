using NHibernate;
using ISession = NHibernate.ISession;

namespace Todo.Core.Common.UnitOfWork;

public abstract class BaseUnitOfWork<TUnit, TSession> : IBaseUnitOfWork<TSession>
    where TUnit : class, IBaseUnitOfWork<TSession> where TSession : IDisposable
{
    protected Lazy<TSession> _lazySession;

    private class UowWrapper<TUOW>
    {
        public UowWrapper(TUOW uow)
        {
            UnitOfWork = uow;
        }

        public TUOW UnitOfWork { get; set; }
    }

    private static AsyncLocal<UowWrapper<TUnit?>> _current =
        new AsyncLocal<UowWrapper<TUnit?>>
        {
            Value = null
        };

    private IBaseUnitOfWork<TSession>? _parent;

    public IBaseUnitOfWork<TSession>? Parent => _parent;

    public static IBaseUnitOfWork<TSession>? Current => _current.Value?.UnitOfWork;

    protected BaseUnitOfWork()
    {
        _parent = _current.Value?.UnitOfWork;
        _current.Value = new UowWrapper<TUnit?>(this as TUnit);
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
        if (_current.Value != null)
        {
            _current.Value.UnitOfWork = _parent as TUnit;
        }
        
        GetCurrentSession()?.Dispose();
        OnDispose();
    }

    public ValueTask DisposeAsync()
    {
        Dispose();
        return new ValueTask(Task.CompletedTask);
    }
}