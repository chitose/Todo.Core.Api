namespace Todo.Core.Common.UnitOfWork;

public abstract class BaseUnitOfWork<TUnit, TSession> : IBaseUnitOfWork<TSession>
    where TUnit : class, IBaseUnitOfWork<TSession> where TSession : IDisposable
{
    private static readonly AsyncLocal<UowWrapper<TUnit?>> _current =
        new()
        {
            Value = null!
        };

    protected Lazy<TSession> LazySession = null!;

    protected BaseUnitOfWork()
    {
        Parent = _current.Value?.UnitOfWork;
        _current.Value = new UowWrapper<TUnit?>(this as TUnit);
    }

    public static IBaseUnitOfWork<TSession>? Current => _current.Value?.UnitOfWork;

    public IBaseUnitOfWork<TSession>? Parent { get; }

    public abstract ISessionAccessor GetCurrentSession();

    public void Dispose()
    {
        if (_current.Value != null) _current.Value.UnitOfWork = Parent as TUnit;

        GetCurrentSession().Dispose();
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

    private class UowWrapper<TUow>
    {
        public UowWrapper(TUow uow)
        {
            UnitOfWork = uow;
        }

        public TUow UnitOfWork { get; set; }
    }
}