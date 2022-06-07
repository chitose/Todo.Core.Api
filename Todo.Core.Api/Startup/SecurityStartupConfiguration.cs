using Todo.Core.Common.Startup;

namespace Todo.Core.Api.Startup;

public class SecurityStartupConfiguration : IAppStartupConfiguration
{
    public int Order => StartupOrder.Security;
    public void ConfigureApp(WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}