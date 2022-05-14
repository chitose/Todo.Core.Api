using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Mapping;

public abstract class BaseEntityMapping<T> : ClassMapping<T> where T : BaseEntity
{
    protected BaseEntityMapping()
    {
        Id(x => x.Id, c =>
        {
            c.Column("id");
            c.Generator(Generators.Identity);
        });
        Version(x => x.Version, c =>
        {
            c.Column("version");
            c.Type(NHibernateUtil.Int32);
        });
        Property(x => x.CreatedAt, c => { c.Column("created_at"); });
        Property(x => x.ModifiedAt, c => { c.Column("modified_at"); });
    }
}