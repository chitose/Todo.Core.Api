namespace Todo.Core.Persistence.Entities;

public interface IAuditableEntity
{
    DateTime CreatedAt { get; set; }

    DateTime ModifiedAt { get; set; }
}