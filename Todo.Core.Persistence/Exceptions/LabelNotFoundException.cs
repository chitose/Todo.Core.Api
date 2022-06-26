namespace Todo.Core.Persistence.Exceptions;

public class LabelNotFoundException : EntityNotFoundException
{
    public LabelNotFoundException(int key) : base(key, "Label")
    {
    }
}