using Microsoft.AspNetCore.Identity;

namespace Todo.Core.Service.User;

public interface IUserService
{
    Task<Persistence.Entities.User> GetUserById(string id);
    
    Task<Persistence.Entities.User> GetUserByUserName(string id);

    Task<Persistence.Entities.User> CreateUser(Persistence.Entities.User user, string password);
}