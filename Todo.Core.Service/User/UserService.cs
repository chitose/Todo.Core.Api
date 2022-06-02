using Todo.Core.Persistence.Repositories;

namespace Todo.Core.Service.User;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
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
}