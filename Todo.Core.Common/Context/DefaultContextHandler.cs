using System.Collections;

namespace Todo.Core.Common.Context;

public class DefaultContextHandler : IContextHandler
{
    private readonly AsyncLocal<Hashtable> _local = new() {Value = new Hashtable()};

    public T? GetValue<T>(string key)
    {
        var val = _local.Value![key];
        if (val != null) return (T) val;

        return default;
    }

    public void SetValue<T>(string key, T value)
    {
        _local.Value![key] = value;
    }

    public void CreateChildContext()
    {
        // copy also the dictionary to not modify the dictionary of the caller.
        var existing = _local.Value;
        _local.Value = existing != null ? new Hashtable(existing) : new Hashtable();
    }
}