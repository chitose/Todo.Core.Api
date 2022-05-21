using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Repositories;

public interface ITaskRepository : IGenericRepository<TodoTask>
{
}