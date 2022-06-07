using Serilog;
using Todo.Core.Common.Startup;

namespace Todo.Core.Common.Logging;

public class SerilogStartupConfigurationConfiguration : IBuilderStartupConfiguration, IAppStartupConfiguration
{
    public int Order => 1;

    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((ct, lc) => { lc.ReadFrom.Configuration(ct.Configuration); });
    }

    public void ConfigureApp(WebApplication app)
    {
        app.UseSerilogRequestLogging();
    }
}