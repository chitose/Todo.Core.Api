namespace Todo.Core.Persistence.Entities;

public class Label : BaseEntity, IOrderable
{
    public virtual string Title { get; set; }

    public virtual bool Shared { get; set; }

    public virtual User Owner { get; set; }

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual ICollection<TodoTask> Tasks { get; set; } = new List<TodoTask>();

    public virtual int Order { get; set; }
}