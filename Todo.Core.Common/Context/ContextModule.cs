using Autofac;

namespace Todo.Core.Common.Context;

public class ContextModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<DefaultContextHandler>().As<IContextHandler>();
    }
}