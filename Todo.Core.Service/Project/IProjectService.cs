using Todo.Core.Domain.Project;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Service.Project;

public interface IProjectService
{
    Task<Persistence.Entities.Project> CreateProject(ProjectCreationInfo creationInfo);

    Task<Persistence.Entities.Project> GetProject(int id);

    Task<Persistence.Entities.Project> UpdateProject(int id, ProjectUpdateInfo updateInfo);

    Task DeleteProject(int id);

    Task<List<Persistence.Entities.Project>> GetProjects(bool archived = false);

    Task SwapProjectOrder(int source, int target);

    Task<ProjectComment> AddComment(int projectId, string content);

    Task<List<ProjectComment>> LoadComments(int projectId);
}