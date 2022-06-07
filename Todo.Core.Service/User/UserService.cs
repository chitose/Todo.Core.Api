using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Todo.Core.Persistence.Repositories;

namespace Todo.Core.Service.User;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly SignInManager<Persistence.Entities.User> _signInManager;

    public UserService(IUserRepository userRepository, SignInManager<Persistence.Entities.User> signInManager)
    {
        _userRepository = userRepository;
        _signInManager = signInManager;
    }

    public Task<Persistence.Entities.User> GetUserById(string id)
    {
        return _userRepository.FindById(id);
    }

    public Task<Persistence.Entities.User> GetUserByUserName(string username)
    {
        return _userRepository.FindByUserName(username);
    }

    public Task<Persistence.Entities.User> CreateUser(Persistence.Entities.User user, string password)
    {
        return _userRepository.CreateUser(user, password);
    }

    public Task<Persistence.Entities.User> CreateExtUser(Persistence.Entities.User user)
    {
        return _userRepository.CreateExtUser(user);
    }

    public Task<Persistence.Entities.User> GetUser(ClaimsPrincipal identity)
    {
        return _userRepository.GetUser(identity);
    }

    public Task SignIn(Persistence.Entities.User user, string authenticationMethod)
    {
        return _signInManager.SignInAsync(user, false, authenticationMethod);
    }
}