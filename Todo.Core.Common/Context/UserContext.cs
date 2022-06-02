using System.Collections;

namespace Todo.Core.Common.Context;

public static class UserContext
{
    private static IContextHandler _contextHandler;

    public static string? UserName
    {
        get => _contextHandler.GetValue<string>(nameof(UserName));
        set => _contextHandler.SetValue(nameof(UserName), value);
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
}