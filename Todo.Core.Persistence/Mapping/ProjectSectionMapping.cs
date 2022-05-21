using NHibernate.Mapping.ByCode;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Mapping;

public class ProjectSectionMapping : BaseEntityMapping<ProjectSection>
{
    public ProjectSectionMapping()
    {
        Table("project_section");

        Property(x => x.Order, c => { c.Column("[order]"); });

        Property(x => x.Title, c =>
        {
            c.Column("name");
            c.Length(256);
        });

        ManyToOne(x => x.Project, m =>
        {
            m.Cascade(Cascade.None);
            m.Lazy(LazyRelation.Proxy);
            m.Column("project_id");
            m.ForeignKey("section_project_fk");
            m.NotNullable(true);
            m.Fetch(FetchKind.Join);
        });

        Set(x => x.Tasks, m =>
        {
            m.Lazy(CollectionLazy.Lazy);
            m.Cascade(Cascade.None);
            m.Inverse(true);
            m.Key(x =>
            {
                x.Column("section_id");
                x.ForeignKey("section_task_fk");
            });
        }, r => r.OneToMany());
    }
}