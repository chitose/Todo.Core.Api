using NHibernate.Cfg;
using NHibernate.Envers.Configuration;
using NHibernate.Envers.Configuration.Fluent;
using Todo.Core.Persistence.Entities;
using Todo.Core.Persistence.SessionFactory;

namespace Todo.Core.Persistence.DatabaseConfiguration;

public class AuditConfiguration : INhibernateDatabaseConfiguration
{
    public bool AfterMapping => true;

    public void Configure(Configuration config)
    {
        var enversConf = new FluentConfiguration();
        enversConf.Audit<Project>()
            .Exclude(p => p.Comments)
            .Exclude(p => p.Users)
            .Exclude(p => p.Sections);
        enversConf.Audit<Label>()
            .Exclude(x => x.Owner);
        enversConf.Audit<TodoTask>()
            .Exclude(x => x.Section)
            .Exclude(x => x.ParentTask)
            .Exclude(x => x.AssignedTo)
            .Exclude(x => x.Comments);

        config.IntegrateWithEnvers(enversConf);
    }
}