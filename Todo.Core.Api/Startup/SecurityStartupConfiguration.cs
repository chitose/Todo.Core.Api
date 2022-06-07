using Microsoft.AspNetCore.Identity;
using Todo.Core.Api.Startup.Security;
using Todo.Core.Common.Startup;

namespace Todo.Core.Api.Startup;

public class SecurityStartupConfiguration : IAppStartupConfiguration, IBuilderStartupConfiguration
{
    private IEnumerable<IExternalAuthenticationConfiguration> _externalAuthenticationConfigurations;

    public SecurityStartupConfiguration(IEnumerable<IExternalAuthenticationConfiguration> extAuthConfigs)
    {
        _externalAuthenticationConfigurations = extAuthConfigs;
    }

    public int Order => StartupOrder.Security;

    public void ConfigureApp(WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }

    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        var authBuilder = builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
        });

        authBuilder.AddCookie(opt => opt.LoginPath = "/auth");

        authBuilder.AddIdentityCookies();

        foreach (var c in _externalAuthenticationConfigurations)
        {
            c.Configure(authBuilder);
        }
    }
}