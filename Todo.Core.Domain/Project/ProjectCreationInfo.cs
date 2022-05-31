using Todo.Core.Domain.Enum;

namespace Todo.Core.Domain.Project;

public class ProjectCreationInfo : ProjectUpdateInfo
{
    public bool? Default { get; set; }

    public new string Name { get; set; }

    public new ProjectView View { get; set; }

    public bool? Archived { get; set; }

    public int? AboveProject { get; set; }

    public int? BelowProject { get; set; }
}