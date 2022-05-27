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
        builder.Services.AddAuthentication()
            .AddGoogle(opts =>
            {
                opts.ClientId = _configProvider.GetConfigValue("Authentication_Google_ClientId");
                opts.ClientSecret = _configProvider.GetConfigValue("Authentication_Google_ClientSecret");
            });
    }

    public void ConfigureApp(WebApplication app)
    {
        // no thing
    }
}