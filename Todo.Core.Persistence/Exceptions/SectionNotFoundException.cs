namespace Todo.Core.Persistence.Exceptions;

public class SectionNotFoundException : EntityNotFoundException
{
    public SectionNotFoundException(int sectId) : base(sectId, "section")
    {
    }
}