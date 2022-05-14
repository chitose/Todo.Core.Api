using NHibernate.Mapping.ByCode;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Mapping;

public class UserMapping : BaseEntityMapping<User>
{
    public UserMapping()
    {
        Table("user");
        const string indexName = "user_idx";
        Property(x => x.UserId, c =>
        {
            c.Column("user_id");
            c.Unique(true);
            c.NotNullable(true);
        });
        Property(x => x.Photo, c => { c.Column("photo"); });
        Property(x => x.DisplayName, c =>
        {
            c.Column("display_name");
            c.Length(255);
            c.Index(indexName);
        });
        Property(x => x.FirstName, c =>
        {
            c.Column("first_name");
            c.Length(255);
            c.Index(indexName);
        });
        Property(x => x.LastName, c =>
        {
            c.Column("last_name");
            c.Index(indexName);
            c.Length(255);
        });
        Property(x => x.Email, c =>
        {
            c.Column("email");
            c.Length(255);
        });

        Set(x => x.Projects, opt =>
        {
            opt.Table("user_project");
            opt.Key(x =>
            {
                x.Column("user_id");
                x.NotNullable(true);
            });
            opt.Cascade(Cascade.DeleteOrphans | Cascade.All);
            opt.Lazy(CollectionLazy.Lazy);
            opt.Inverse(true);
        }, opt => { opt.ManyToMany(x => x.Column("project_id")); });
    }
}