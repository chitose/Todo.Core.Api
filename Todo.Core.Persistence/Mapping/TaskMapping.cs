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

        Set(x => x.SubTasks, m =>
        {
            m.Lazy(CollectionLazy.Lazy);
            m.Cascade(Cascade.All);
            m.Inverse(true);
            m.Key(x =>
            {
                x.Column("parent_id");
                x.ForeignKey("task_subtasks_fk");
            });
        }, r => r.OneToMany());

        ManyToOne(x => x.ParentTask, m =>
        {
            m.Column("parent_id");
            m.ForeignKey("subtask_task_fk");
            m.Cascade(Cascade.None);
            m.Lazy(LazyRelation.Proxy);
        });

        ManyToOne(x => x.Project, m =>
        {
            m.Column("project_id");
            m.ForeignKey("task_project_fk");
            m.Cascade(Cascade.None);
            m.NotNullable(true);
            m.Lazy(LazyRelation.Proxy);
        });

        ManyToOne(x => x.Section, m =>
        {
            m.Column("section_id");
            m.ForeignKey("task_section_fk");
            m.Cascade(Cascade.None);
            m.Lazy(LazyRelation.Proxy);
        });

        ManyToOne(x => x.AssignedTo, m =>
        {
            m.Lazy(LazyRelation.Proxy);
            m.Cascade(Cascade.None);
            m.Column("assigned");
            m.ForeignKey("task_user_fk");
        });

        Set(x => x.Labels, colm =>
        {
            colm.Table("task_label");
            colm.Key(c =>
            {
                c.Column("task_id");
                c.ForeignKey("task_label_fk");
                c.NotNullable(true);
            });
            colm.Cascade(Cascade.None);
            colm.Lazy(CollectionLazy.Lazy);
        }, col => { col.ManyToMany(x => x.Column("label_id")); });

        Set(x => x.Comments, c =>
        {
            c.Key(x =>
            {
                x.NotNullable(true);
                x.Column("target_id");
                x.ForeignKey("task_comment_fk");
            });
            c.Lazy(CollectionLazy.Lazy);
            c.Cascade(Cascade.All);
            c.Inverse(true);
        }, c => { c.OneToMany(); });
    }
}