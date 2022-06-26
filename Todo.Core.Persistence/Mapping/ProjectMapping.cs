using NHibernate.Mapping.ByCode;
using Todo.Core.Domain.Enum;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Mapping;

public class ProjectMapping : BaseEntityMapping<Project>
{
    public ProjectMapping()
    {
        Table("project");
        const string indexName = "project_idx";
        Property(x => x.Archived, c =>
        {
            c.Column("archived");
            c.Index(indexName);
        });
        Property(x => x.Default, opt =>
        {
            opt.Column(c =>
            {
                c.Name("[default]");
                c.Default(0);
            });
            opt.NotNullable(false);
        });
        Property(x => x.Name, c =>
        {
            c.Column("name");
            c.Length(255);
            c.NotNullable(true);
            c.Index(indexName);
        });
        Property(x => x.View, c =>
        {
            c.Column(x =>
            {
                x.Name("[view]");
                x.Default((byte) ProjectView.List);
            });
        });
        Property(x => x.GroupBy, c =>
        {
            c.Column("group_by");
            c.Length(50);
        });

        Property(x => x.SortBy, c =>
        {
            c.Column("sort_by");
            c.Length(50);
        });
        Property(x => x.SortAsc, c =>
        {
            c.Column(x =>
            {
                x.Name("sort_asc");
                x.Default(1);
            });
        });
        Property(x => x.ShowCompleted, c =>
        {
            c.Column(x =>
            {
                x.Name("show_completed");
                x.Default(0);
            });
        });

        Property(x => x.Order, c =>
        {
            c.Column(ci =>
            {
                ci.Name("[order]");
                ci.Default(0);
            });
        });

        Bag(x => x.Comments, c =>
        {
            c.Key(x =>
            {
                x.NotNullable(true);
                x.Column("target_id");
                x.ForeignKey("project_comment_fk");
            });
            c.Lazy(CollectionLazy.Lazy);
            c.Cascade(Cascade.Remove);
            c.Inverse(true);
        }, c => { c.OneToMany(); });


        Bag(x => x.UserProjects, colm =>
        {
            colm.Key(c => { c.Column("project_id"); });
            colm.Inverse(true);
            colm.Cascade(Cascade.Merge.Include(Cascade.DeleteOrphans).Include(Cascade.Remove));
        }, col => { col.OneToMany(); });

        Bag(x => x.Labels, colm =>
        {
            colm.Table("project_label");
            colm.Key(c =>
            {
                c.Column("project_id");
                c.ForeignKey("project_label_fk");
                c.NotNullable(true);
            });
            colm.Lazy(CollectionLazy.Lazy);
        }, col => { col.ManyToMany(x => x.Column("label_id")); });

        Bag(x => x.Sections, m =>
        {
            m.Lazy(CollectionLazy.Lazy);
            m.Fetch(CollectionFetchMode.Join);
            m.Cascade(Cascade.Remove);
            m.Inverse(true);
            m.Key(c =>
            {
                c.Column("project_id");
                c.ForeignKey("project_section_fk");
            });
        }, r => { r.OneToMany(); });

        Bag(x => x.Tasks, m =>
            {
                m.Lazy(CollectionLazy.Lazy);
                m.Cascade(Cascade.Remove);
                m.Inverse(true);
                m.Key(c =>
                {
                    c.Column("project_id");
                    c.ForeignKey("project_task_fk");
                });
            },
            r => r.OneToMany());
    }
}