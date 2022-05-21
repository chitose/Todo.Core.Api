using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Mapping;

public class ProjectCommentMapping : SubclassMapping<ProjectComment>
{
    public ProjectCommentMapping()
    {
        DiscriminatorValue(@"Project");

        ManyToOne(x => x.Project, c =>
        {
            c.Column("target_id");
            c.ForeignKey("project_comment_fk");
            c.NotNullable(true);
            c.Cascade(Cascade.None);
            c.Lazy(LazyRelation.Proxy);
        });
    }
}