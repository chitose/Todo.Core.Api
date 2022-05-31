using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Repositories;

public interface IUserRepository
{
    Task<User> FindById(string id);

    Task<User> FindByUserName(string username);

    Task<User> CreateUser(User user, string password);
}