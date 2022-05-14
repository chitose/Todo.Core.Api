using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Repositories;

public class UserRepository : GenericEntityRepository<User>, IUserRepository
{
    
}