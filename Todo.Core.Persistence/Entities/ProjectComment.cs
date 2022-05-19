namespace Todo.Core.Persistence.Entities;

public class ProjectComment : BaseComment
{
    public virtual Project Project { get; set; }
}