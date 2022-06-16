using System.Reflection.Emit;
using Todo.Core.Domain.Enum;

namespace Todo.Core.Domain.Project;

public class TaskCreationInfo
{
    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime? DueDate { get; set; }

    public TaskPriority Priority { get; set; } = TaskPriority.Normal;

    public ICollection<Label> Labels { get; set; } = new List<Label>();

    public int? ParentTask { get; set; }

    public int? Section { get; set; }

    public int? Project { get; set; }

    public int Order { get; set; }
    
    public int? AboveTask { get; set; }
    
    public int? BelowTask { get; set; }
}