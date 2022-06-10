using AutoMapper;
using NHibernate.Linq;
using Todo.Core.Common.Context;
using Todo.Core.Common.Exception;
using Todo.Core.Common.UnitOfWork;
using Todo.Core.Domain.Project;
using Todo.Core.Persistence.Entities;
using Todo.Core.Persistence.Exceptions;
using Todo.Core.Persistence.Extensions;
using Todo.Core.Persistence.Repositories;
using Todo.Core.Service.User;

namespace Todo.Core.Service.Project;

public class ProjectService : IProjectService
{
    private readonly IMapper _mapper;
    private readonly ICommentRepository<ProjectComment> _projectCommentRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectSectionRepository _projectSectionRepository;
    private readonly IUnitOfWorkProvider _unitOfWorkProvider;
    private readonly IUserService _userService;

    public ProjectService(ICommentRepository<ProjectComment> projectCommentRepository,
        IProjectRepository projectRepository, IUnitOfWorkProvider unitOfWorkProvider,
        IProjectSectionRepository projectSectionRepository,
        IMapper mapper, IUserService userService)
    {
        _projectRepository = projectRepository;
        _projectCommentRepository = projectCommentRepository;
        _unitOfWorkProvider = unitOfWorkProvider;
        _mapper = mapper;
        _projectSectionRepository = projectSectionRepository;
        _userService = userService;
    }

    public Task<Persistence.Entities.Project> ArchiveProject(int projectId,
        CancellationToken cancellationToken = default)
    {
        return _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            var prj = await _projectRepository.GetByKey(projectId, cancellationToken);
            if (prj == null)
            {
                throw new ProjectNotFoundException(projectId);
            }

            prj.Archived = true;

            return await _projectRepository.Save(prj, cancellationToken);
        });
    }

    public Task<Persistence.Entities.Project> UnarchiveProject(int projectId,
        CancellationToken cancellationToken = default)
    {
        return _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            var prj = await _projectRepository.GetByKey(projectId, cancellationToken);
            if (prj == null)
            {
                throw new ProjectNotFoundException(projectId);
            }

            prj.Archived = false;

            return await _projectRepository.Save(prj, cancellationToken);
        });
    }

    public Task InviteUserToProject(int projectId, string userName, CancellationToken cancellationToken = default)
    {
        return _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            var prj = await _projectRepository.GetByKey(projectId, cancellationToken);
            if (prj == null)
            {
                throw new ProjectNotFoundException(projectId);
            }

            var user = await _userService.GetUserByUserName(userName);
            if (user == null)
            {
                throw new UserNotFoundException(userName);
            }

            if (!prj.UserProjects.Any(u => u.User.UserName == user.UserName))
            {
                var up = new UserProject
                {
                    User = user,
                    Project = prj,
                    JoinedTime = DateTime.UtcNow
                };
                await UnitOfWork.Current.GetCurrentSession().PersistAsync(up, cancellationToken);
                prj.UserProjects.Add(up);
                await _projectRepository.Save(prj, cancellationToken);
            }
        });
    }

    public Task RemoveUserFromProject(int projectId, string userName, CancellationToken cancellationToken = default)
    {
        return _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            var prj = await _projectRepository.GetByKey(projectId, cancellationToken);
            if (prj == null)
            {
                throw new ProjectNotFoundException(projectId);
            }

            ValidateProjectLeave(prj);

            var u = prj.UserProjects.FirstOrDefault(x => x.User.UserName == userName);

            if (u != null)
            {
                await UnitOfWork.Current.GetCurrentSession().DeleteAsync(u, cancellationToken);
                prj.UserProjects.Remove(u);
                await _projectRepository.Save(prj, cancellationToken);
            }
        });
    }

    public Task LeaveProject(int projectId, CancellationToken cancellationToken = default)
    {
        return _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            var prj = await _projectRepository.GetAll()
                .Fetch(x => x.UserProjects)
                .FirstOrDefaultAsync(x => x.Id == projectId, cancellationToken);
            if (prj == null)
            {
                throw new ProjectNotFoundException(projectId);
            }

            ValidateProjectLeave(prj);

            var u = prj.UserProjects.FirstOrDefault(x => x.User.UserName == UserContext.UserName);
            if (u != null)
            {
                await UnitOfWork.Current.GetCurrentSession().DeleteAsync(u, cancellationToken);
                prj.UserProjects.Remove(u);
                var newOwner = prj.UserProjects.MinBy(x => x.JoinedTime);
                newOwner.Owner = true;
                await UnitOfWork.Current.GetCurrentSession().PersistAsync(newOwner, cancellationToken);
                await _projectRepository.Save(prj, cancellationToken);
            }
        });
    }

    public async Task SwapProjectOrder(int source, int target, CancellationToken cancellationToken = default)
    {
        await _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            await _projectRepository.SwapEntityOrder(source, target, cancellationToken);
        });
    }

    public async Task<ProjectComment> AddComment(int projectId, string content,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            var project = await _projectRepository.GetByKey(projectId, cancellationToken);
            if (project == null)
            {
                throw new ProjectNotFoundException(projectId);
            }

            return await _projectCommentRepository.Add(new ProjectComment
            {
                Content = content,
                Project = project
            }, cancellationToken);
        });
    }

    public Task<List<ProjectComment>> LoadComments(int projectId, CancellationToken cancellationToken = default)
    {
        return _unitOfWorkProvider.PerformActionInUnitOfWork(() =>
        {
            var comments = _projectCommentRepository.GetAll()
                .Where(x => x.Project.Id == projectId)
                .ToListAsync(cancellationToken);
            return comments;
        });
    }

    private static void ValidateProjectLeave(Persistence.Entities.Project prj)
    {
        if (prj.UserProjects.Count == 1)
        {
            throw new TodoException("Cannot leave project without any collaborator.");
        }
    }

    #region CRUD

    public Task<Persistence.Entities.Project> CreateProject(ProjectCreationInfo creationInfo,
        CancellationToken cancellationToken = default)
    {
        if (creationInfo.AboveProject.HasValue && creationInfo.BelowProject.HasValue)
        {
            throw new TodoException(
                "Invalid project creation Info. Cannot add a project above and below other project at the same time.");
        }

        if (string.IsNullOrWhiteSpace(creationInfo.Name))
        {
            throw new ArgumentNullException("Project name is required.", nameof(creationInfo.Name));
        }

        return _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            var project = new Persistence.Entities.Project
            {
                Name = creationInfo.Name,
                View = creationInfo.View,
                Default = creationInfo.Default ?? false,
                Archived = creationInfo.Archived ?? false,
                GroupBy = creationInfo.GroupBy ?? string.Empty,
                SortBy = creationInfo.SortBy ?? string.Empty,
                SortAsc = creationInfo.SortAsc ?? false,
                Order = await _projectRepository.CalculateNewEntityOrder(creationInfo.AboveProject,
                    creationInfo.BelowProject, cancellationToken)
            };
            return await _projectRepository.Add(project, cancellationToken);
        });
    }

    public Task<Persistence.Entities.Project?> GetProject(int id, CancellationToken cancellationToken = default)
    {
        return _unitOfWorkProvider.PerformActionInUnitOfWork(() => _projectRepository.GetByKey(id, cancellationToken));
    }

    public Task<Persistence.Entities.Project> UpdateProject(int id, ProjectUpdateInfo updateInfo,
        CancellationToken cancellationToken = default)
    {
        return _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            var project = await _projectRepository.GetByKey(id, cancellationToken);
            if (project == null)
            {
                throw new ProjectNotFoundException(id);
            }

            _mapper.Map(updateInfo, project);

            await _projectRepository.Save(project, cancellationToken);

            return project;
        });
    }

    public Task DeleteProject(int id, CancellationToken cancellationToken = default)
    {
        return _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            var prj = await _projectRepository.GetByKey(id, cancellationToken);
            if (prj == null)
            {
                throw new ProjectNotFoundException(id);
            }

            if (prj.AuthorId != UserContext.UserName
                || prj.UserProjects.Count > 1)
            {
                throw new TodoException("Only project owner can delete the project");
            }

            return _projectRepository.DeleteByKey(id, cancellationToken);
        });
    }

    public Task<List<Persistence.Entities.Project>> GetProjects(bool archived = false,
        CancellationToken cancellationToken = default)
    {
        return _unitOfWorkProvider.PerformActionInUnitOfWork(() =>
            _projectRepository.GetAll()
                .Where(x => archived && x.Archived || !archived && !x.Archived).ToListAsync(cancellationToken)
        );
    }

    #endregion

    #region Section

    public Task<ProjectSection> AddSection(string title, int? aboveSection = null, int? belowSection = null,
        CancellationToken cancellationToken = default)
    {
        return _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            var sect = new ProjectSection
            {
                Title = title,
                Order = await _projectSectionRepository.CalculateNewEntityOrder(aboveSection, belowSection,
                    cancellationToken)
            };

            return await _projectSectionRepository.Add(sect, cancellationToken);
        });
    }

    public Task<ProjectSection> UpdateSection(int sectId, string title, CancellationToken cancellationToken = default)
    {
        return _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            var sect = await _projectSectionRepository.GetByKey(sectId, cancellationToken);
            if (sect == null)
            {
                throw new SectionNotFoundException(sectId);
            }

            sect.Title = title;
            await _projectSectionRepository.Save(sect, cancellationToken);
            return sect;
        });
    }

    public Task SwapSectionOrder(int source, int target, CancellationToken cancellationToken = default)
    {
        return _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            await _projectSectionRepository.SwapEntityOrder(source, target, cancellationToken);
        });
    }

    public async Task<ProjectSection> ArchiveSection(int sectionId, CancellationToken cancellationToken = default)
    {
        // set archive = true
        // mark this section's tasks as done
        return await _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            var sect = await _projectSectionRepository
                .GetAll()
                .Fetch(x => x.Tasks)
                .FirstOrDefaultAsync(x => x.Id == sectionId, cancellationToken);

            if (sect == null)
            {
                throw new SectionNotFoundException(sectionId);
            }

            sect.Archived = true;
            foreach (var t in sect.Tasks) t.Completed = true;

            return sect;
        });
    }

    public Task<ProjectSection> UnarchiveSection(int sectionId, CancellationToken cancellationToken = default)
    {
        // set archive = false
        // no task status update
        return _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            var sect = await _projectSectionRepository.GetByKey(sectionId, cancellationToken);
            if (sect == null)
            {
                throw new SectionNotFoundException(sectionId);
            }

            sect.Archived = false;
            return await _projectSectionRepository.Save(sect, cancellationToken);
        });
    }

    public Task DeleteSection(int sectionId, CancellationToken cancellationToken = default)
    {
        // delete the task
        return _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            var sect = await _projectSectionRepository.GetByKey(sectionId, cancellationToken);
            if (sect == null)
            {
                throw new SectionNotFoundException(sectionId);
            }

            await _projectSectionRepository.Delete(sect, cancellationToken);
        });
    }

    public Task<List<ProjectSection>> LoadSections(int projectId, CancellationToken cancellationToken = default)
    {
        return _unitOfWorkProvider.PerformActionInUnitOfWork(() => _projectSectionRepository.GetAll()
            .Where(x => x.Project.Id == projectId).ToListAsync(cancellationToken));
    }

    #endregion
}