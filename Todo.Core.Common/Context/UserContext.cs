using System.Collections;

namespace Todo.Core.Common.Context;

public static class UserContext
{
    //private static readonly AsyncLocal<Hashtable> CtxStorage = new(ValueChangedHandler) {Value = new Hashtable()};

    private static IContextHandler _contextHandler;

    public static string? UserId
    {
        get => _contextHandler.GetValue<string>(nameof(UserId));
        set => _contextHandler.SetValue(nameof(UserId), value);
    }

    public static string? UserDisplayName
    {
        get => _contextHandler.GetValue<string>(nameof(UserDisplayName));
        set => _contextHandler.SetValue(nameof(UserDisplayName), value);
    }

    public static void InitializeContext(IContextHandler contextHandler)
    {
        _contextHandler = contextHandler;
    }

    private static void ValueChangedHandler(AsyncLocalValueChangedArgs<Hashtable> evt)
    {
        if (evt.ThreadContextChanged && evt.PreviousValue != null && evt.CurrentValue == null)
        {
            // kind of a work-around issue when thread context changed while
            // using tasks.
            // as long as no new value is assigned, here we simple restore the previous value
            // CtxStorage.Value = evt.PreviousValue;
        }
    }
}