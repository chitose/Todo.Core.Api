using Autofac;
using Todo.Core.Common.Startup;

namespace Todo.Core.Api.Startup;

public class StartupModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<GoogleAuth>().As<IBuilderStartupConfiguration>();
        builder.RegisterType<SwaggerStartupConfiguration>()
            .As<IBuilderStartupConfiguration>()
            .As<IAppStartupConfiguration>();
        builder.RegisterType<SecurityStartupConfiguration>()
            .As<IAppStartupConfiguration>();
        builder.RegisterType<ControllerStartupConfiguration>()
            .As<IBuilderStartupConfiguration>()
            .As<IAppStartupConfiguration>();
        builder.RegisterType<DevConfigurationStartupConfiguration>()
            .As<IBuilderStartupConfiguration>();
        builder.RegisterType<HttpsRedirectionStartupConfiguration>()
            .As<IAppStartupConfiguration>();
    }
}