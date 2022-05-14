using NHibernate.Engine;
using NHibernate.Mapping.ByCode.Conformist;
using Todo.Core.Persistence.Entities;
using Cascade = NHibernate.Mapping.ByCode.Cascade;

namespace Todo.Core.Persistence.Mapping;

public class ProjectCommentMapping : SubclassMapping<ProjectComment>
{
    public ProjectCommentMapping()
    {
        DiscriminatorValue(@"Project");

        ManyToOne(x => x.Project, c =>
        {
            c.Column("project_id");
            c.NotNullable(true);
            c.Cascade(Cascade.Persist);
        });
    }
}