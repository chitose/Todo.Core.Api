using Todo.Core.Common.Exception;

namespace Todo.Core.Persistence.Exceptions;

public class EntityNotFoundException : TodoException
{
    public EntityNotFoundException(int key, string name) : base(
        $"The entity {name} with key = {key} could not be found")
    {
    }
}