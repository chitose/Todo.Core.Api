using Todo.Core.Domain.Project;
using Todo.Core.Persistence.Repositories;

namespace Todo.Core.Service.TodoTask;

public class TodoTaskService : ITodoTaskService
{
    private readonly IProjectRepository _projectRepository;
    private readonly ITaskRepository _taskRepository;
    
    public TodoTaskService(IProjectRepository projectRepository, ITaskRepository taskRepository)
    {
        _projectRepository = projectRepository;
        _taskRepository = taskRepository;
    }
    public Task<Persistence.Entities.TodoTask> CreateTask(int projectId, TaskCreationInfo creationInfo, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}