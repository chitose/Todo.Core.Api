using AutoMapper;
using NHibernate.Linq;
using Todo.Core.Common.Exception;
using Todo.Core.Common.UnitOfWork;
using Todo.Core.Domain.Project;
using Todo.Core.Persistence.Entities;
using Todo.Core.Persistence.Repositories;

namespace Todo.Core.Service.Project;

public class ProjectService : IProjectService
{
    private readonly IMapper _mapper;
    private readonly ICommentRepository<ProjectComment> _projectCommentRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWorkProvider _unitOfWorkProvider;

    public ProjectService(ICommentRepository<ProjectComment> projectCommentRepository,
        IProjectRepository projectRepository, IUnitOfWorkProvider unitOfWorkProvider,
        IMapper mapper)
    {
        _projectRepository = projectRepository;
        _projectCommentRepository = projectCommentRepository;
        _unitOfWorkProvider = unitOfWorkProvider;
        _mapper = mapper;
    }

    public Task<Persistence.Entities.Project> CreateProject(ProjectCreationInfo creationInfo)
    {
        if (creationInfo.AboveProject.HasValue && creationInfo.BelowProject.HasValue)
        {
            throw new TodoException(
                $"Invalid project creation Info. Cannot add a project above and below other project at the same time.");
        }

        return _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            var projectOrder = 0;
            if (creationInfo.AboveProject.HasValue || creationInfo.BelowProject.HasValue)
            {
                var targetProject = await
                    _projectRepository.GetByKey(creationInfo.AboveProject ?? creationInfo.BelowProject!.Value);

                var otherProjectsQuery = _projectRepository.GetAll();
                if (creationInfo.AboveProject.HasValue)
                {
                    projectOrder = targetProject.Order - 1;
                    otherProjectsQuery = otherProjectsQuery.Where(x => x.Order < targetProject.Order);
                }
                else
                {
                    projectOrder = targetProject.Order + 1;
                    otherProjectsQuery = otherProjectsQuery.Where(x => x.Order > targetProject.Order);
                }

                var otherProjects = await otherProjectsQuery.ToListAsync();
                otherProjects.ForEach(p =>
                {
                    p.Order += (creationInfo.AboveProject.HasValue ? -1 : 1) * 1;
                    _projectRepository.Save(p);
                });
            }
            else
            {
                projectOrder = (await _projectRepository.GetAll().MaxAsync(x => x.Order)) + 1;
            }

            var project = new Persistence.Entities.Project
            {
                Name = creationInfo.Name,
                View = creationInfo.View,
                Default = creationInfo.Default ?? false,
                Archived = creationInfo.Archived ?? false,
                GroupBy = creationInfo.GroupBy ?? string.Empty,
                SortBy = creationInfo.SortBy ?? string.Empty,
                SortAsc = creationInfo.SortAsc ?? false,
                Order = projectOrder
            };
            return await _projectRepository.Add(project);
        });
    }

    public Task<Persistence.Entities.Project> GetProject(int id)
    {
        return _unitOfWorkProvider.PerformActionInUnitOfWork(() => _projectRepository.GetByKey(id));
    }

    public Task<Persistence.Entities.Project> UpdateProject(int id, ProjectUpdateInfo updateInfo)
    {
        return _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            var project = await _projectRepository.GetByKey(id);
            if (project == null)
            {
                throw new ProjectNotFoundException(id);
            }

            _mapper.Map(updateInfo, project);

            await _projectRepository.Save(project);

            return project;
        });
    }

    public Task DeleteProject(int id)
    {
        return _unitOfWorkProvider.PerformActionInUnitOfWork(() => _projectRepository.DeleteByKey(id));
    }

    public Task<List<Persistence.Entities.Project>> GetProjects(bool archived = false)
    {
        return _unitOfWorkProvider.PerformActionInUnitOfWork(() =>
        {
            return _projectRepository.GetAll()
                .Where(x => (archived && x.Archived) || (!archived && !x.Archived)).ToListAsync();
        });
    }

    public async Task SwapProjectOrder(int source, int target)
    {
        await _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            var projects = await _projectRepository.GetAll()
                .Where(x => x.Id == source || x.Id == target).ToListAsync();

            var sp = projects.FirstOrDefault(p => p.Id == source);
            var tp = projects.FirstOrDefault(p => p.Id == target);
            if (sp == null)
            {
                throw new ProjectNotFoundException(source);
            }

            if (tp == null)
            {
                throw new ProjectNotFoundException(target);
            }

            (sp.Order, tp.Order) = (tp.Order, sp.Order);

            await _projectRepository.Save(sp);
            await _projectRepository.Save(tp);
        });
    }
}