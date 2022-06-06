using Autofac;
using Todo.Core.Common.Startup;

namespace Todo.Core.Api.Startup;

public class StartupModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<GoogleAuth>().As<IStartupConfiguration>();
    }
}