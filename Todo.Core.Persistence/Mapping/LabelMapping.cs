using NHibernate.Mapping.ByCode;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Mapping;

public class LabelMapping : BaseEntityMapping<Label>
{
    public LabelMapping()
    {
        Table("label");
        var idx_name = "label_idx";

        Property(x => x.Title, c =>
        {
            c.Column("title");
            c.Length(256);
            c.Index(idx_name);
        });

        Property(x => x.Shared, c =>
        {
            c.Column("shared");
            c.Index(idx_name);
        });

        Property(x => x.Order, c => { c.Column("[order]"); });

        ManyToOne(x => x.Owner, m =>
        {
            m.Column("owner_id");
            m.ForeignKey("owner_fk");
            m.Lazy(LazyRelation.Proxy);
            m.NotNullable(true);
            m.Cascade(Cascade.None);
        });

        Bag(x => x.Projects, colm =>
        {
            colm.Table("project_label");
            colm.Key(c =>
            {
                c.Column("label_id");
                c.NotNullable(true);
            });
            colm.Cascade(Cascade.None);
            colm.Lazy(CollectionLazy.Lazy);
        }, col => { col.ManyToMany(x => x.Column("project_id")); });

        Bag(x => x.Tasks, colm =>
        {
            colm.Table("task_label");
            colm.Key(c =>
            {
                c.Column("label_id");
                c.NotNullable(true);
            });
            colm.Cascade(Cascade.None);
            colm.Lazy(CollectionLazy.Lazy);
        }, col => { col.ManyToMany(x => x.Column("task_id")); });
    }
}