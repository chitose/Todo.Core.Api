using AutoMapper;
using Todo.Core.Common.UnitOfWork;
using Todo.Core.Domain.Project;
using Todo.Core.Persistence.Exceptions;
using Todo.Core.Persistence.Extensions;
using Todo.Core.Persistence.Repositories;

namespace Todo.Core.Service.TodoTask;

public class TodoTaskService : ITodoTaskService
{
    private readonly IMapper _mapper;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectSectionRepository _projectSectionRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWorkProvider _unitOfWorkProvider;

    public TodoTaskService(IProjectRepository projectRepository, ITaskRepository taskRepository, IMapper mapper,
        IUnitOfWorkProvider unitOfWorkProvider, IProjectSectionRepository projectSectionRepository)
    {
        _projectRepository = projectRepository;
        _taskRepository = taskRepository;
        _mapper = mapper;
        _unitOfWorkProvider = unitOfWorkProvider;
        _projectSectionRepository = projectSectionRepository;
    }

    public async Task<Persistence.Entities.TodoTask> CreateTask(TaskCreationInfo creationInfo,
        CancellationToken cancellationToken = default)
    {
        if (creationInfo.Project == null)
            throw new ArgumentNullException(nameof(creationInfo.Project), "Project is required.");

        if (creationInfo.AboveTask.HasValue && creationInfo.BelowTask.HasValue)
            throw new ArgumentException($"Cannot have both {creationInfo.AboveTask} and {creationInfo.BelowTask}.");

        return await _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            var prj = await _projectRepository.GetByKey(creationInfo.Project.Value, cancellationToken);

            if (prj == null) throw new ProjectNotFoundException(creationInfo.Project.Value);

            var task = _mapper.Map<Persistence.Entities.TodoTask>(creationInfo);
            if (creationInfo.Section.HasValue)
            {
                var sect = await _projectSectionRepository.GetByKey(creationInfo.Section.Value,
                    cancellationToken);

                if (sect == null) throw new SectionNotFoundException(creationInfo.Section.Value);

                task.Section = sect;
            }

            task.Project = prj;
            task.Order =
                await _taskRepository.CalculateNewEntityOrder(creationInfo.AboveTask, creationInfo.BelowTask,
                    cancellationToken);
            return await _taskRepository.Add(task, cancellationToken);
        });
    }

    public Task<Persistence.Entities.TodoTask> GetByKey(int taskId, CancellationToken cancellationToken = default)
    {
        return _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
            await _taskRepository.GetByKey(taskId, cancellationToken));
    }
}