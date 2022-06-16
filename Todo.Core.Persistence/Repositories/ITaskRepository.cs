using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Repositories;

public interface ITaskRepository : IGenericRepository<TodoTask>
{
    Task<TodoTask> CloneTask(TodoTask todoTask, ProjectSection section, CancellationToken cancellationToken);
}