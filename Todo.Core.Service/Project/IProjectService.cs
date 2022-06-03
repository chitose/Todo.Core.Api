using Todo.Core.Domain.Project;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Service.Project;

public interface IProjectService
{
    Task<Persistence.Entities.Project> CreateProject(ProjectCreationInfo creationInfo,
        CancellationToken cancellationToken = default);

    Task<Persistence.Entities.Project?> GetProject(int id, CancellationToken cancellationToken = default);

    Task<Persistence.Entities.Project> UpdateProject(int id, ProjectUpdateInfo updateInfo,
        CancellationToken cancellationToken = default);

    Task DeleteProject(int id, CancellationToken cancellationToken = default);

    Task<List<Persistence.Entities.Project>> GetProjects(bool archived = false,
        CancellationToken cancellationToken = default);

    Task<Persistence.Entities.Project> ArchiveProject(int projectId, CancellationToken cancellationToken = default);

    Task<Persistence.Entities.Project> UnarchiveProject(int projectId, CancellationToken cancellationToken = default);

    Task InviteUserToProject(int projectId, string userName, CancellationToken cancellationToken = default);

    Task RemoveUserFromProject(int projectId, string userName, CancellationToken cancellationToken = default);

    Task LeaveProject(int projectId, CancellationToken cancellationToken = default);

    Task SwapProjectOrder(int source, int target, CancellationToken cancellationToken = default);

    Task<ProjectComment> AddComment(int projectId, string content, CancellationToken cancellationToken = default);

    Task<List<ProjectComment>> LoadComments(int projectId, CancellationToken cancellationToken = default);

    Task<ProjectSection> AddSection(string title, int? aboveSection = null, int? belowSection = null,
        CancellationToken cancellationToken = default);

    Task<ProjectSection> UpdateSection(int sectId, string title, CancellationToken cancellationToken = default);

    Task SwapSectionOrder(int source, int target, CancellationToken cancellationToken = default);

    /// <summary>
    /// Archive the section, mark all of its subtask as completed.
    /// </summary>
    /// <param name="sectionId">Section id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    Task<ProjectSection> ArchiveSection(int sectionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Unarchive the section, no changes in sub tasks
    /// </summary>
    /// <param name="sectionId">Section id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    Task<ProjectSection> UnarchiveSection(int sectionId, CancellationToken cancellationToken = default);

    Task DeleteSection(int sectionId, CancellationToken cancellationToken = default);

    Task<List<ProjectSection>> LoadSections(int projectId, CancellationToken cancellationToken = default);
}