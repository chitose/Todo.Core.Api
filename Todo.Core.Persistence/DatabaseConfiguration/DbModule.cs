using Autofac;
using Todo.Core.Persistence.SessionFactory;

namespace Todo.Core.Persistence.DatabaseConfiguration;

public class DbModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<SqliteDbConfiguration>().As<INhibernateDatabaseConfiguration>();
        builder.RegisterType<AuditConfiguration>().As<INhibernateDatabaseConfiguration>();
    }
}