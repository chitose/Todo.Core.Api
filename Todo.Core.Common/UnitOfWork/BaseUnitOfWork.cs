namespace Todo.Core.Common.UnitOfWork;

public abstract class BaseUnitOfWork<TUnit, TSession> : IBaseUnitOfWork<TSession>
    where TUnit : class, IBaseUnitOfWork<TSession> where TSession : IDisposable
{
    private static readonly AsyncLocal<UowWrapper<TUnit?>> _current =
        new()
        {
            Value = null
        };

    protected Lazy<TSession> _lazySession;

    protected BaseUnitOfWork()
    {
        Parent = _current.Value?.UnitOfWork;
        _current.Value = new UowWrapper<TUnit?>(this as TUnit);
    }

    public static IBaseUnitOfWork<TSession>? Current => _current.Value?.UnitOfWork;

    public IBaseUnitOfWork<TSession>? Parent { get; }

    public TSession GetCurrentSession()
    {
        return _lazySession.Value;
    }

    public void Dispose()
    {
        if (_current.Value != null) _current.Value.UnitOfWork = Parent as TUnit;

        GetCurrentSession()?.Dispose();
        OnDispose();
    }

    public ValueTask DisposeAsync()
    {
        Dispose();
        return new ValueTask(Task.CompletedTask);
    }

    protected virtual void OnDispose()
    {
        // to be override by sub classes
    }

    private class UowWrapper<TUOW>
    {
        public UowWrapper(TUOW uow)
        {
            UnitOfWork = uow;
        }

        public TUOW UnitOfWork { get; set; }
    }
}