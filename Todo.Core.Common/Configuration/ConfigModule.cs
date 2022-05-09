using Autofac;

namespace Todo.Core.Common.Configuration;

public class ConfigModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<JsonConfigProvider>().As<IConfigProvider>().SingleInstance();
    }
}