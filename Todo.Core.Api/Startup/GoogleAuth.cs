using Microsoft.AspNetCore.Authentication.Cookies;
using Todo.Core.Common.Configuration;
using Todo.Core.Common.Startup;

namespace Todo.Core.Api.Startup;

public class GoogleAuth : IBuilderStartupConfiguration
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
                _configProvider.Bind("Authentication:Google", opts);
            });
    }

    public void ConfigureApp(WebApplication app)
    {
        // no thing
    }
}