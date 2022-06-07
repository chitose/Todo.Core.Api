using Microsoft.AspNetCore.Authentication;
using Todo.Core.Common.Configuration;

namespace Todo.Core.Api.Startup.Security;

public class TwitterAuth : IExternalAuthenticationConfiguration
{
    private readonly IConfigProvider _configProvider;
    public TwitterAuth(IConfigProvider configProvider)
    {
        _configProvider = configProvider;
    }
    public void Configure(AuthenticationBuilder builder)
    {
        builder.AddTwitter(options =>
        {
            _configProvider.Bind("Authentication:Twitter", options);
            options.RetrieveUserDetails = true;
            options.ClaimActions.MapJsonKey("profile_image_url_https","profile_image_url_https", "url");
            options.ClaimActions.MapJsonKey("name","name", "text");
        });
    }
}