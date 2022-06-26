using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using Todo.Core.Common.Configuration;
using Todo.Core.Common.UnitOfWork;
using Todo.Core.Persistence.SessionFactory;

namespace Todo.Core.Persistence.DatabaseConfiguration;

public class SqliteDbConfiguration : INhibernateDatabaseConfiguration, ISessionListener
{
    private readonly IConfigProvider _configProvider;


    public SqliteDbConfiguration(IConfigProvider configProvider)
    {
        _configProvider = configProvider;
    }

    public bool AfterMapping => false;

    public void Configure(Configuration config)
    {
        config.DataBaseIntegration(db =>
        {
            db.ConnectionString = _configProvider.GetConnectionString("todo-sqlite");
            db.Dialect<SQLiteDialect>();
            db.Driver<SQLite20Driver>();
            db.LogSqlInConsole = true;
        });
    }

    public string ConnectionType => "SQLITE";

    public void OnOpen(ISessionAccessor accessor)
    {
        var sql = accessor.CreateSql("PRAGMA foreign_keys = ON;");
        sql.ExecuteUpdate();
    }
}