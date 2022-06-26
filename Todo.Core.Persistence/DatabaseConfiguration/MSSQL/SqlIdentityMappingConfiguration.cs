using NHibernate.AspNetCore.Identity;
using NHibernate.Cfg;
using Todo.Core.Persistence.SessionFactory;

namespace Todo.Core.Persistence.DatabaseConfiguration;

public class SqlIdentityMappingConfiguration : INhibernateDatabaseConfiguration
{
    public bool AfterMapping => false;

    public void Configure(Configuration config)
    {
        config.AddIdentityMappingsForMsSql();
    }

    public string ConnectionType => "MSSQL";
}