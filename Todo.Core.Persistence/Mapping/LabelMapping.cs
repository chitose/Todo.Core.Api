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
        
        Property(x=>x.Order, c =>
        {
            c.Column("[order]");
        });
        
        Set(x => x.Projects, colm =>
        {
            colm.Table("project_label");
            colm.Key(c =>
            {
                c.Column("label_id");
                c.NotNullable(true);
            });
            colm.Cascade(Cascade.None);
            colm.Lazy(CollectionLazy.Lazy);
            colm.Inverse(true);
        }, col => { col.ManyToMany(x => x.Column("project_id")); });
        
        Set(x => x.Tasks, colm =>
        {
            colm.Table("task_label");
            colm.Key(c =>
            {
                c.Column("label_id");
                c.NotNullable(true);
            });
            colm.Cascade(Cascade.None);
            colm.Lazy(CollectionLazy.Lazy);
            colm.Inverse(true);
        }, col => { col.ManyToMany(x => x.Column("task_id")); });
    }
}