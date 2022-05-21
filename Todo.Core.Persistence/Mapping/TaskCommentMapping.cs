using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Mapping;

public class TaskCommentMapping : SubclassMapping<TaskComment>
{
    public TaskCommentMapping()
    {
        DiscriminatorValue(@"Task");

        ManyToOne(x => x.Task, m =>
        {
            m.Column("target_id");
            m.ForeignKey("comment_task_fk");
            m.NotNullable(true);
            m.Cascade(Cascade.None);
            m.Lazy(LazyRelation.Proxy);
        });
    }
}