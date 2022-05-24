using Todo.Core.Domain.Enum;

namespace Todo.Core.Persistence.Entities;

public class TodoTask : BaseEntity
{
    public virtual string Title { get; set; }

    public virtual string Description { get; set; }

    public virtual DateTime? DueDate { get; set; }

    public virtual TaskPriority Priority { get; set; } = TaskPriority.Normal;

    public virtual int Order { get; set; }

    public virtual User? AssignedTo { get; set; }

    public virtual bool Completed { get; set; }

    public virtual ICollection<Label> Labels { get; set; } = new List<Label>();

    public virtual TodoTask? ParentTask { get; set; }

    public virtual ICollection<TodoTask> SubTasks { get; set; } = new List<TodoTask>();

    public virtual ProjectSection? Section { get; set; }

    public virtual Project? Project { get; set; }

    public virtual ICollection<TaskComment> Comments { get; set; } = new List<TaskComment>();
}