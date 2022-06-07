using Autofac;
using Todo.Core.Common.Startup;

namespace Todo.Core.Api.Startup;

public class StartupModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<SwaggerStartupConfiguration>()
            .As<IBuilderStartupConfiguration>()
            .As<IAppStartupConfiguration>();
        builder.RegisterType<SecurityStartupConfiguration>()
            .As<IAppStartupConfiguration>()
            .As<IBuilderStartupConfiguration>();
        builder.RegisterType<ControllerStartupConfiguration>()
            .As<IBuilderStartupConfiguration>()
            .As<IAppStartupConfiguration>();
        builder.RegisterType<HttpsRedirectionStartupConfiguration>().As<IAppStartupConfiguration>();
        builder.RegisterType<HttpContextAccessorStartupConfiguration>().As<IBuilderStartupConfiguration>();
    }
}