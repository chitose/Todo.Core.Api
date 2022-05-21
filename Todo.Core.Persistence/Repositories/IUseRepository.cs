using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User> GetByUserId(string testUserId);
}