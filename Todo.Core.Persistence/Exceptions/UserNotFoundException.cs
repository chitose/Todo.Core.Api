using Todo.Core.Common.Exception;

namespace Todo.Core.Persistence.Exceptions;

public class UserNotFoundException : TodoException
{
    public UserNotFoundException(string username) : base($"The user [{username}] could not be found.")
    {
    }
}