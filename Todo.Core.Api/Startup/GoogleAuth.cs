using Microsoft.AspNetCore.Authentication.Cookies;
using Todo.Core.Common.Configuration;
using Todo.Core.Common.Startup;

namespace Todo.Core.Api.Startup;

public class GoogleAuth : IStartupConfiguration
{
    private IConfigProvider _configProvider;

    public GoogleAuth(IConfigProvider configProvider)
    {
        _configProvider = configProvider;
    }
    
    public int Order => 1;
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(opts => opts.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(opts =>
            {
                opts.LoginPath = "/auth/google";
            })
            .AddGoogle(opts =>
            {
                opts.ClientId = _configProvider.GetConfigValue("Authentication:Google_ClientId");
                opts.ClientSecret = _configProvider.GetConfigValue("Authentication:Google_ClientSecret");
                opts.CallbackPath = "/auth/google/callback";
            });
    }

    public void ConfigureApp(WebApplication app)
    {
        // no thing
    }
}