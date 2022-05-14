using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Todo.Core.Persistence.Entities;
using Todo.Core.Persistence.Enum;

namespace Todo.Core.Persistence.Mapping;

public class ProjectMapping : BaseEntityMapping<Project>
    {
        public ProjectMapping()
        {
            Table("project");
            const string indexName = "project_idx";
            Property(x => x.Archived, c =>
            {
                c.Column("isArchived");
                c.Index(indexName);
            });
            Property(x => x.Default, opt =>
            {
                opt.Column(c =>
                {
                    c.Name("isDefault");
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
            Set(x => x.Comments, c =>
            {
                c.Key(x =>
                {
                    x.NotNullable(true);
                    x.Column("project_id");
                });
                c.Lazy(CollectionLazy.Lazy);
                c.Cascade(Cascade.All | Cascade.DeleteOrphans);
                c.Inverse(true);
            }, c => { c.OneToMany(); });

            Set(x => x.Users, colm =>
            {
                colm.Table("user_project");
                colm.Key(c =>
                {
                    c.Column("project_id");
                    c.NotNullable(true);
                });
                colm.Cascade(Cascade.All | Cascade.DeleteOrphans);
                colm.Lazy(CollectionLazy.Lazy);
                colm.Inverse(true);
            }, col => { col.ManyToMany(x => x.Column("user_id")); });
        }
    }