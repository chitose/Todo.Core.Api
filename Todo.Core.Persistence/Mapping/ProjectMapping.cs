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
                c.Default(false);
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
                x.Name("view");
                x.Default(ProjectView.List);
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
                x.Default(true);
            });
        });
        Property(x => x.ShowCompleted, c =>
        {
            c.Column(x =>
            {
                x.Name("show_completed");
                x.Default(false);
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

        Set(x => x.Comments, c =>
        {
            c.Key(x =>
            {
                x.NotNullable(true);
                x.Column("target_id");
                x.ForeignKey("project_comment_fk");
            });
            c.Lazy(CollectionLazy.Lazy);
            c.Cascade(Cascade.All);
            c.Inverse(true);
        }, c => { c.OneToMany(); });

        Set(x => x.Users, colm =>
        {
            colm.Table("user_project");
            colm.Key(c =>
            {
                c.Column("project_id");
                c.NotNullable(true);
                c.ForeignKey("project_user_fk");
            });
            colm.Cascade(Cascade.None);
            colm.Lazy(CollectionLazy.Lazy);
            colm.Inverse(true);
        }, col => { col.ManyToMany(x => x.Column("user_id")); });

        Set(x => x.Labels, colm =>
        {
            colm.Table("project_label");
            colm.Key(c =>
            {
                c.Column("project_id");
                c.ForeignKey("project_label_fk");
                c.NotNullable(true);
            });
            colm.Cascade(Cascade.None);
            colm.Lazy(CollectionLazy.Lazy);
            colm.Inverse(true);
        }, col => { col.ManyToMany(x => x.Column("label_id")); });

        Set(x => x.Sections, m =>
        {
            m.Lazy(CollectionLazy.Lazy);
            m.Fetch(CollectionFetchMode.Join);
            m.Cascade(Cascade.All);
            m.Inverse(true);
            m.Key(c =>
            {
                c.Column("project_id");
                c.ForeignKey("project_section_fk");
            });
        }, r => { r.OneToMany(); });
    }
}