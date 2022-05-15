using System.Collections;

namespace Todo.Core.Common.Context;

public static class UserContext
{
    private static readonly AsyncLocal<Hashtable> CtxStorage = new AsyncLocal<Hashtable>();

    private static void SetData(string key, object data)
    {
        if (CtxStorage.Value == null)
        {
            CtxStorage.Value = new Hashtable();
        }

        CtxStorage.Value[key] = data;
    }
    public static int UserId { 
        get => (int)CtxStorage.Value![nameof(UserId)]!;
        set => SetData(nameof(UserId),value);
    }

    public static string UserName
    {
        get => (string)CtxStorage.Value![nameof(UserName)]!;
        set => SetData(nameof(UserName),value);
    }
}