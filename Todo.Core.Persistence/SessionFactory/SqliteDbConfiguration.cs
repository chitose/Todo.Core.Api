using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using Todo.Core.Common.Configuration;

namespace Todo.Core.Persistence.SessionFactory;

public class SqliteDbConfiguration : INhibernateDatabaseConfiguration
{
    private readonly IConfigProvider _configProvider;
    public SqliteDbConfiguration(IConfigProvider configProvider)
    {
        _configProvider = configProvider;
    }
    public void Configure(Configuration config)
    {
        SQLitePCL.Batteries_V2.Init();
        config.DataBaseIntegration(db =>
        {
            db.ConnectionString = _configProvider.GetConnectionString("todo");
            db.Dialect<SQLiteDialect>();
            db.Driver<SQLite20Driver>();
        });
    }
}