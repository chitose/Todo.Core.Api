using Todo.Core.Domain.Project;

namespace Todo.Core.Service.TodoTask;

public interface ITodoTaskService
{
    Task<Persistence.Entities.TodoTask> CreateTask(int projectId, TaskCreationInfo creationInfo,
        CancellationToken cancellationToken = default);
}