namespace Todo.Core.Persistence.Entities;

public interface IAuditableEntity
{
    DateTime CreatedAt { get; set; }

    DateTime ModifiedAt { get; set; }
    
    string Author { get; set; }
    
    string Editor { get; set; }
}