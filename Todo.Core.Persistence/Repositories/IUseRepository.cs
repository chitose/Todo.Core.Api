using System.Security.Claims;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Repositories;

public interface IUserRepository
{
    Task<User> FindById(string id);

    Task<User> FindByUserName(string username);

    Task<User> CreateUser(User user, string password);
    
    Task<User> GetUser(ClaimsPrincipal identity);
    Task<User> CreateExtUser(User user);
    Task UpdateUser(User user);
}