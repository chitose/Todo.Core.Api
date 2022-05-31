using Microsoft.AspNetCore.Identity;

namespace Todo.Core.Service.User;

public interface IUserService
{
    Task<Persistence.Entities.User> GetUserById(string id, CancellationToken cancellationToken = default);

    Task<IdentityResult> CreateUser(Persistence.Entities.User user);
}