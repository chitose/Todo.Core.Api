using Todo.Core.Domain.Enum;

namespace Todo.Core.Persistence.Entities;

public class Project : BaseEntity, IOrderable
{
    public virtual string Name { get; set; }

    public virtual ProjectView View { get; set; } = ProjectView.List;

    public virtual bool ShowCompleted { get; set; }

    public virtual string GroupBy { get; set; }

    public virtual string SortBy { get; set; }

    public virtual bool SortAsc { get; set; }

    public virtual bool Default { get; set; }

    public virtual bool Archived { get; set; }

    public virtual ICollection<ProjectComment> Comments { get; set; } = new List<ProjectComment>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<Label> Labels { get; set; } = new List<Label>();

    public virtual ICollection<TodoTask> Tasks { get; set; } = new List<TodoTask>();

    public virtual ICollection<ProjectSection> Sections { get; set; } = new List<ProjectSection>();

    public virtual int Order { get; set; }
}