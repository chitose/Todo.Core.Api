using NHibernate.AspNetCore.Identity;
using NHibernate.Cfg;
using Todo.Core.Persistence.SessionFactory;

namespace Todo.Core.Persistence.DatabaseConfiguration;

public class IdentityMappingConfiguration : INhibernateDatabaseConfiguration
{
    public bool AfterMapping => false;

    public void Configure(Configuration config)
    {
        config.AddIdentityMappingsForSqlite();
    }
}