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
        
        enversConf.Audit<Project>().Exclude(p => p.Comments)
            .Exclude(p => p.Users);
        enversConf.Audit<Label>();

        config.IntegrateWithEnvers(enversConf);
    }
}