using Todo.Core.Common.Startup;

namespace Todo.Core.Api.Startup;

public class HttpsRedirectionStartupConfiguration : IAppStartupConfiguration
{
    public int Order => StartupOrder.Security - 1;
    public void ConfigureApp(WebApplication app)
    {
        app.UseHttpsRedirection();
    }
}