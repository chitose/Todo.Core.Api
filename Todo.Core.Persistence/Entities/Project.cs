using Todo.Core.Persistence.Enum;

namespace Todo.Core.Persistence.Entities;

public class Project : BaseEntity
{
    public virtual string Name { get; set; }

    public virtual ProjectView View { get; set; } = ProjectView.List;

    public virtual bool ShowCompleted { get; set; }

    public virtual string GroupBy { get; set; }

    public virtual string SortBy { get; set; }

    public virtual bool SortAsc { get; set; }

    public virtual bool Default { get; set; }

    public virtual bool Archived { get; set; }

    public virtual ICollection<ProjectComment> Comments { get; protected set; } = new List<ProjectComment>();

    public virtual ICollection<User> Users { get; protected set; } = new List<User>();

    public virtual ICollection<Label> Labels { get; protected set; } = new List<Label>();
}