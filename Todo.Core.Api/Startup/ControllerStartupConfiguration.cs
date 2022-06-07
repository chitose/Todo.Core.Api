using Todo.Core.Common.Startup;

namespace Todo.Core.Api.Startup;

public class ControllerStartupConfiguration : IBuilderStartupConfiguration, IAppStartupConfiguration
{
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        // Add services to the container.
        builder.Services.AddControllers();
    }

    public int Order => StartupOrder.Security + 1;
    public void ConfigureApp(WebApplication app)
    {
        app.MapControllers();
    }
}