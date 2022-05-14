namespace Todo.Core.Persistence.Entities;

public class BaseComment : BaseEntity
{
    public virtual string Content { get; set; }
}