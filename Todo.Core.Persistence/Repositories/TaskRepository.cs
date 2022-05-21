using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Repositories;

public class TaskRepository : GenericEntityRepository<TodoTask>, ITaskRepository
{
}