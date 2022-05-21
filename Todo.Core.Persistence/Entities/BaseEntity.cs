namespace Todo.Core.Persistence.Entities;

public abstract class BaseEntity : IAuditableEntity
{
    public virtual int Id { get; protected set; }

    public virtual int Version { get; set; }

    public virtual DateTime CreatedAt { get; set; }

    public virtual DateTime ModifiedAt { get; set; }

    public virtual string Author { get; set; }

    public virtual string Editor { get; set; }
}