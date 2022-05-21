using Autofac;
using Todo.Core.Common.Startup;

namespace Todo.Core.Common.Logging;

public class LoggingModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<SerilogStartupConfigurationConfiguration>().As<IStartupConfiguration>()
            .InstancePerDependency();
    }
}