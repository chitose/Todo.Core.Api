using Autofac;

namespace Todo.Core.Persistence.Repositories;

public class RepositoryModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(GenericEntityRepository<>))
            .As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
        builder.RegisterType<UserRepository>().As<IUserRepository>();
    }
}