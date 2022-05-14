namespace Todo.Core.Persistence.Entities;

public class User : BaseEntity
{
    public virtual string DisplayName { get; set; }
    public virtual string FirstName { get; set; }
    public virtual string LastName { get; set; }
    public virtual string Photo { get; set; }
    public virtual string Email { get; set; }
    public virtual string UserId { get; set; }

    public virtual ICollection<Project> Projects { get; protected set; } = new List<Project>();
}