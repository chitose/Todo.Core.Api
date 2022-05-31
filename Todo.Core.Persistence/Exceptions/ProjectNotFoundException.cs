namespace Todo.Core.Persistence.Exceptions;

public class ProjectNotFoundException : EntityNotFoundException
{
    public ProjectNotFoundException(int id) : base(id, "project")
    {
    }
}