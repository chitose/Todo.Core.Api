using Todo.Core.Domain.Project;

namespace Todo.Core.Service.Project;

public class ProjectService : IProjectService
{
    public Task<Persistence.Entities.Project> CreateProject(ProjectCreationInfo creationInfo)
    {
        throw new NotImplementedException();
    }

    public Task<Persistence.Entities.Project> UpdateProject(ProjectUpdateInfo updateInfo)
    {
        throw new NotImplementedException();
    }

    public Task DeleteProject(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IList<Persistence.Entities.Project>> GetProjects(bool includeArchived = false)
    {
        throw new NotImplementedException();
    }
}