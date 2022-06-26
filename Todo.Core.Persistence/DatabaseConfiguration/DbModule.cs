using Autofac;
using Todo.Core.Common.UnitOfWork;
using Todo.Core.Persistence.SessionFactory;

namespace Todo.Core.Persistence.DatabaseConfiguration;

public class DbModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AuditConfiguration>().As<INhibernateDatabaseConfiguration>();

        builder.RegisterType<SqliteIdentityMappingConfiguration>().As<INhibernateDatabaseConfiguration>();
        builder.RegisterType<SqliteDbConfiguration>().As<INhibernateDatabaseConfiguration>()
            .As<ISessionListener>();

        builder.RegisterType<SqlDbConfiguration>().As<INhibernateDatabaseConfiguration>();
        builder.RegisterType<SqlIdentityMappingConfiguration>().As<INhibernateDatabaseConfiguration>();
    }
}