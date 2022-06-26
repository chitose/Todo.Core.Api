using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Mapping;

public abstract class BaseEntityMapping<T> : ClassMapping<T> where T : BaseEntity
{
    protected BaseEntityMapping()
    {
        Schema("dbo");
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
        Property(x => x.AuthorId, c => { c.Column("author_id"); });
        Property(x => x.Author, c => { c.Column("author"); });
        Property(x => x.EditorId, c => { c.Column("editor_id"); });
        Property(x => x.Editor, c => { c.Column("editor"); });
    }
}