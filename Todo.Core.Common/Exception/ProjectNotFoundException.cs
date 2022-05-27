namespace Todo.Core.Common.Exception;

public class ProjectNotFoundException : TodoException
{
    public ProjectNotFoundException(int id) : base($"The project with id = {id} could not be found")
    {
    }
}