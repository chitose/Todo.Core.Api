using Microsoft.AspNetCore.Identity;
using NHibernate;
using NHibernate.AspNetCore.Identity;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Identity;

public class TodoUserStore : UserStore<User, UserRole>
{
    public TodoUserStore(ISession session, IdentityErrorDescriber errorDescriber) : base(session, errorDescriber)
    {
    }
}