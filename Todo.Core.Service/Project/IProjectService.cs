using Todo.Core.Domain.Project;

namespace Todo.Core.Service.Project;

public interface IProjectService
{
    Task<Todo.Core.Persistence.Entities.Project> CreateProject(ProjectCreationInfo creationInfo);

    Task<Todo.Core.Persistence.Entities.Project> UpdateProject(ProjectUpdateInfo updateInfo);

    Task DeleteProject(int id);

    Task<IList<Todo.Core.Persistence.Entities.Project>> GetProjects(bool includeArchived = false);
}