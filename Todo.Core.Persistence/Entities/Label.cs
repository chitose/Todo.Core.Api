namespace Todo.Core.Persistence.Entities;

public class Label : BaseEntity
{
    public virtual string Title { get; set; }
    
    public virtual int Order { get; set; }
    
    public virtual bool Shared { get; set; }

    public virtual ICollection<Project> Projects { get; protected set; } = new List<Project>();
}