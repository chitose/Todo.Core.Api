namespace Todo.Core.Persistence.Entities;

public class User : BaseEntity
{
    public virtual string DisplayName { get; set; }
    public virtual string FirstName { get; set; }
    public virtual string LastName { get; set; }
    public virtual string Photo { get; set; }
    public virtual string Email { get; set; }
    public virtual string? UserId { get; set; }

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual ICollection<TodoTask> Tasks { get; set; } = new List<TodoTask>();

    public virtual ICollection<Label> Labels { get; set; } = new List<Label>();
}