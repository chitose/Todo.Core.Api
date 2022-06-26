using Todo.Core.Domain.Project;

namespace Todo.Core.Service.TodoTask;

public interface ITodoTaskService
{
    Task<Persistence.Entities.TodoTask> CreateTask(TaskCreationInfo creationInfo,
        CancellationToken cancellationToken = default);

    Task<Persistence.Entities.TodoTask> GetByKey(int taskId, CancellationToken cancellationToken = default);
}