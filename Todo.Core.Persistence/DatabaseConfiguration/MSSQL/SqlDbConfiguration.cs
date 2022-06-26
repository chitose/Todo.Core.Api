using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using Todo.Core.Common.Configuration;
using Todo.Core.Persistence.SessionFactory;

namespace Todo.Core.Persistence.DatabaseConfiguration;

public class SqlDbConfiguration : INhibernateDatabaseConfiguration
{
    private readonly IConfigProvider _configProvider;


    public SqlDbConfiguration(IConfigProvider configProvider)
    {
        _configProvider = configProvider;
    }

    public bool AfterMapping => false;

    public void Configure(Configuration config)
    {
        config.DataBaseIntegration(db =>
        {
            db.ConnectionString = _configProvider.GetConnectionString("todo-mssql");
            db.Dialect<MsSql2008Dialect>();
            db.Driver<Sql2008ClientDriver>();
            db.LogSqlInConsole = true;
        });
    }

    public string ConnectionType => "MSSQL";
}