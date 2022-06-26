using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Mapping;

public class UserProjectMapping : ClassMapping<UserProject>
{
    public UserProjectMapping()
    {
        Schema("dbo");
        Table("user_project");
        Property(x => x.Owner, c => c.Column("owner"));
        Property(x => x.JoinedTime, c => c.Column("joined"));
        Id(x => x.Id, c =>
        {
            c.Column("id");
            c.Generator(Generators.Identity);
        });

        ManyToOne(x => x.Project, m =>
        {
            m.Column("project_id");
            m.NotNullable(true);
        });

        ManyToOne(x => x.User, m =>
        {
            m.Column("user_id");
            m.NotNullable(true);
        });
    }
}