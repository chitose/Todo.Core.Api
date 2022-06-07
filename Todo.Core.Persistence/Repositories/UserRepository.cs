using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Todo.Core.Persistence.Entities;
using Todo.Core.Persistence.Exceptions;

namespace Todo.Core.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;

    public UserRepository(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public Task<User> FindById(string id)
    {
        return _userManager.FindByIdAsync(id);
    }

    public Task<User> FindByUserName(string username)
    {
        return _userManager.FindByNameAsync(username);
    }

    public async Task<User> CreateUser(User user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            return await _userManager.FindByNameAsync(user.UserName);
        }

        throw new CreateUserException(result.Errors);
    }

    public Task<User> GetUser(ClaimsPrincipal identity)
    {
        return _userManager.GetUserAsync(identity);
    }

    public async Task<User> CreateExtUser(User user)
    {
        var result = await _userManager.CreateAsync(user);
        if (result.Succeeded)
        {
            return await _userManager.FindByNameAsync(user.UserName);
        }

        throw new CreateUserException(result.Errors);
    }

    public Task UpdateUser(User user)
    {
        return _userManager.UpdateAsync(user);
    }
}