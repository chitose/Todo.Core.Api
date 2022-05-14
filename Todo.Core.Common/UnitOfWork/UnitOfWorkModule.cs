using Autofac;

namespace Todo.Core.Common.UnitOfWork;

public class UnitOfWorkModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<UnitOfWorkProvider>().As<IUnitOfWorkProvider>().InstancePerLifetimeScope();
    }
}