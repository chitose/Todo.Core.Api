namespace Todo.Core.Persistence.Entities;

public class ProjectSection : BaseEntity, IOrderable
{
    public virtual string Title { get; set; }

    public virtual bool Archived { get; set; }

    public virtual Project? Project { get; set; }

    public virtual ICollection<TodoTask> Tasks { get; set; } = new List<TodoTask>();

    public virtual int Order { get; set; }
}