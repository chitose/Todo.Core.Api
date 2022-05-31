using Microsoft.AspNetCore.Identity;
using Todo.Core.Persistence.Identity;

namespace Todo.Core.Service.User;

public class UserService : IUserService
{
    private readonly TodoUserManager _userManager;
    private readonly TodoUserStore _userStore;

    public UserService(TodoUserManager todoUserManager, TodoUserStore userStore)
    {
        _userManager = todoUserManager;
        _userStore = userStore;
    }

    public Task<Persistence.Entities.User> GetUserById(string id, CancellationToken cancellationToken = default)
    {
        return _userStore.FindByIdAsync(id, cancellationToken);
    }

    public Task<IdentityResult> CreateUser(Persistence.Entities.User user)
    {
        return _userManager.CreateAsync(user);
    }
}