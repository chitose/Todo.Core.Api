using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Mapping;

public class UserMapping : JoinedSubclassMapping<User>
{
    public UserMapping()
    {
        Extends(typeof(NHibernate.AspNetCore.Identity.IdentityUser));
        ExplicitDeclarationsHolder.AddAsRootEntity(typeof(NHibernate.AspNetCore.Identity.IdentityUser));
        Table("user");
        const string indexName = "user_idx";
        Key(k => k.Column("id"));
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

        Set(x => x.Projects, opt =>
        {
            opt.Table("user_project");
            opt.Key(x =>
            {
                x.Column("user_id");
                x.ForeignKey("project_user_fk");
                x.NotNullable(true);
            });
            opt.Cascade(Cascade.All);
            opt.Lazy(CollectionLazy.Lazy);
        }, opt => { opt.ManyToMany(x => x.Column("project_id")); });

        Set(x => x.Tasks, m =>
        {
            m.Cascade(Cascade.None);
            m.Lazy(CollectionLazy.Lazy);
            m.Inverse(true);
            m.Key(c =>
            {
                c.Column("assigned");
                c.ForeignKey("task_user_fk");
            });
        }, r => r.OneToMany());

        Set(x => x.Labels, m =>
        {
            m.Key(c =>
            {
                c.Column("owner_id");
                c.ForeignKey("label_user_fk");
            });
            m.Cascade(Cascade.All);
            m.Lazy(CollectionLazy.Lazy);
            m.Inverse(true);
        }, r => r.OneToMany());
        
        Set(x=>x.Labels, m =>
        {
            m.Cascade(Cascade.All);
            m.Lazy(CollectionLazy.Lazy);
            m.Inverse(true);
        }, r=>r.OneToMany());
    }
}