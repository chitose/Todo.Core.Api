using NHibernate.Mapping.ByCode.Conformist;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Mapping;

public class UserRoleMapping : JoinedSubclassMapping<UserRole>
{
    public UserRoleMapping()
    {
        ExplicitDeclarationsHolder.AddAsRootEntity(typeof(NHibernate.AspNetCore.Identity.IdentityRole));
        Extends(typeof(NHibernate.AspNetCore.Identity.IdentityRole));
        Table("user_role");
        Key(k => k.Column("id"));
    }
}