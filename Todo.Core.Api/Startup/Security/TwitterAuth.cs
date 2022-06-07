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
            options.Events.OnCreatingTicket = (context) =>
            {
                Console.WriteLine(context.User);
                return Task.CompletedTask;
            };
        });
    }
}