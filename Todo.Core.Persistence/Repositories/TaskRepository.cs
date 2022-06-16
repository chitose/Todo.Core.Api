using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Repositories;

public class TaskRepository : GenericEntityRepository<TodoTask>, ITaskRepository
{
    public Task<TodoTask> CloneTask(TodoTask todoTask, ProjectSection section, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}