using System.Security.Claims;

namespace Todo.Core.Service.User;

public interface IUserService
{
    Task<Persistence.Entities.User> GetUserById(string id);
    
    Task<Persistence.Entities.User> GetUserByUserName(string id);

    Task<Persistence.Entities.User> CreateUser(Persistence.Entities.User user, string password);
    
    Task<Persistence.Entities.User> CreateExtUser(Persistence.Entities.User user);
    
    Task<Persistence.Entities.User> GetUser(ClaimsPrincipal firstOrDefault);
    Task SignIn(Persistence.Entities.User user, string authenticationMethod);
}