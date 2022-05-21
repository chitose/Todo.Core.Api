using System.Collections;
using System.Threading;
using Todo.Core.Common.Context;

namespace Todo.Core.Common.UnitTests;

public class UnitTestContextHandler : IContextHandler
{
    private static readonly AsyncLocal<Hashtable> _local = new(ValueChangedHandler) { Value = new Hashtable() };

    public T? GetValue<T>(string key)
    {
        var val = _local.Value[key];
        if (val != null) return (T)val;

        return default;
    }

    public void SetValue<T>(string key, T value)
    {
        _local.Value[key] = value;
    }

    private static void ValueChangedHandler(AsyncLocalValueChangedArgs<Hashtable> args)
    {
        // work-around to restore previous value whe thread context changed to null
        // due to task is executed in another thread != original thread before await
        if (args.ThreadContextChanged && args.CurrentValue == null && args.PreviousValue != null)
            _local.Value = args.PreviousValue;
    }
}