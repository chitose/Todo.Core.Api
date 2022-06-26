using NHibernate.Mapping.ByCode;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Mapping;

public class TaskMapping : BaseEntityMapping<TodoTask>
{
    public TaskMapping()
    {
        Table("task");
        Property(x => x.Completed, c => { c.Column("done"); });
        Property(x => x.Title, c =>
        {
            c.Column("title");
            c.Length(256);
        });

        Property(x => x.Description, c =>
        {
            c.Column("description");
            c.Length(500);
        });

        Property(x => x.Order, c => { c.Column("[order]"); });

        Property(x => x.Priority, c => { c.Column("priority"); });

        Property(x => x.DueDate, c => { c.Column("due"); });

        Bag(x => x.SubTasks, m =>
        {
            m.Lazy(CollectionLazy.Lazy);
            m.Cascade(Cascade.Remove);
            m.Inverse(true);
            m.Key(x => { x.Column("parent_id"); });
        }, r => r.OneToMany());

        ManyToOne(x => x.ParentTask, m =>
        {
            m.Column("parent_id");
            m.Lazy(LazyRelation.Proxy);
        });

        ManyToOne(x => x.Project, m =>
        {
            m.Column("project_id");
            m.NotNullable(true);
            m.Lazy(LazyRelation.Proxy);
        });

        ManyToOne(x => x.Section, m =>
        {
            m.Column("section_id");
            m.Lazy(LazyRelation.Proxy);
        });

        ManyToOne(x => x.AssignedTo, m =>
        {
            m.Lazy(LazyRelation.Proxy);
            m.Column("assigned");
        });

        Bag(x => x.Labels, colm =>
        {
            colm.Table("task_label");
            colm.Key(c =>
            {
                c.Column("task_id");
                c.NotNullable(true);
            });
            colm.Lazy(CollectionLazy.Lazy);
        }, col => { col.ManyToMany(x => { x.Column("label_id"); }); });

        Bag(x => x.Comments, c =>
        {
            c.Key(x => { x.Column("task_id"); });
            c.Lazy(CollectionLazy.Lazy);
            c.Cascade(Cascade.Remove);
            c.Inverse(true);
        }, c => { c.OneToMany(); });
    }
}