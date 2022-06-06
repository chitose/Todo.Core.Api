namespace Todo.Core.Common.Context;

public class UserContextContent
{
    public string UserId { get; }
    
    public string UserDisplayName { get; }

    public UserContextContent(string userId, string userDisp)
    {
        UserId = userId;
        UserDisplayName = userDisp;
    }
}