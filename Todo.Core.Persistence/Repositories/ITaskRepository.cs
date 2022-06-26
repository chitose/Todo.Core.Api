using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Repositories;

public interface ITaskRepository : IGenericRepository<TodoTask>
{
    Task<TodoTask> CloneTask(TodoTask todoTask, Project? project, ProjectSection? section, TodoTask? parentTask,
        CancellationToken cancellationToken);
}