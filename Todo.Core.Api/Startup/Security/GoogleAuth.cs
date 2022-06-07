using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Todo.Core.Common.Configuration;
using Todo.Core.Common.Startup;

namespace Todo.Core.Api.Startup.Security;

public class GoogleAuth : IExternalAuthenticationConfiguration
{
    private IConfigProvider _configProvider;

    public GoogleAuth(IConfigProvider configProvider)
    {
        _configProvider = configProvider;
    }

    public void Configure(AuthenticationBuilder builder)
    {
        builder.AddGoogle(opts =>
        {
            _configProvider.Bind("Authentication:Google", opts);
            opts.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
            opts.ClaimActions.MapJsonKey("urn:google:locale", "locale", "string");
        });
    }

    public void ConfigureApp(WebApplication app)
    {
        // no thing
    }
}