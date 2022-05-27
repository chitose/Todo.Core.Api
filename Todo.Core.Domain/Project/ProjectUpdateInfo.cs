using Todo.Core.Domain.Enum;

namespace Todo.Core.Domain.Project;

public class ProjectUpdateInfo
{
    public string? Name { get; set; }
    
    public int? Order { get; set; }
    
    public ProjectView? View { get; set; }

    public string? GroupBy { get; set; }
    
    public string? SortBy { get; set; }
    
    public bool? SortAsc { get; set; }
    
    public bool? Archived { get; set; }
    
    public bool? ShowCompleted { get; set; }
}