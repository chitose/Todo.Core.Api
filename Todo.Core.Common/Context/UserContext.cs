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

    public static UserContextContent GetContent()
    {
        return new UserContextContent(UserName, UserDisplayName);
    }

    public static void RestoreFromContent(UserContextContent content)
    {
        UserName = content.UserId;
        UserDisplayName = content.UserDisplayName;
    }

    public static void CreateChildContext()
    {
        _contextHandler.CreateChildContext();
    }
}