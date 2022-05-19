namespace Todo.Core.Persistence.Entities;

public class TaskComment : BaseComment
{
    public virtual TodoTask Task { get; protected set; }
}