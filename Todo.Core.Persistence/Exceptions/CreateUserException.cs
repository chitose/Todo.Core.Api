using Microsoft.AspNetCore.Identity;

namespace Todo.Core.Persistence.Exceptions;

public class CreateUserException : Exception
{
    public CreateUserException(IEnumerable<IdentityError> identityErrors)
    {
        Errors = identityErrors;
    }

    public IEnumerable<IdentityError> Errors { get; }
}