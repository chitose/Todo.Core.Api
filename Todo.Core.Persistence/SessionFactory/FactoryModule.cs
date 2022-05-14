using Autofac;
using NHibernate;

namespace Todo.Core.Persistence.SessionFactory;

public class FactoryModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<SessionFactoryProvider>().SingleInstance();
        builder.Register(c => c.Resolve<SessionFactoryProvider>().CreateFactory()).As<ISessionFactory>()
            .SingleInstance();
        builder.RegisterType<LoggingInterceptor>().SingleInstance();
        builder.RegisterType<SqliteDbConfiguration>().As<INhibernateDatabaseConfiguration>();
    }
}