using NHibernate.Linq;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Repositories;

public class UserRepository : GenericEntityRepository<User>, IUserRepository
{
    public Task<User> GetByUserId(string userId)
    {
        return Session.Query<User>().FirstOrDefaultAsync(x => x.UserId == userId);
    }
}