using System.Collections;

namespace Todo.Core.Common.Context;

public static class UserContext
{
    private static readonly AsyncLocal<Hashtable> CtxStorage = new();
    
    public static int UserId { 
        get => (int)CtxStorage.Value![nameof(UserId)]!;
        set => CtxStorage.Value![nameof(UserId)] = value;
    }

    public static string UserName
    {
        get => (string)CtxStorage.Value![nameof(UserName)]!;
        set => CtxStorage.Value![nameof(UserName)] = value;
    }
}