using Microsoft.AspNetCore.Authentication;

namespace Todo.Core.Api.Startup.Security;

public interface IExternalAuthenticationConfiguration
{
    void Configure(AuthenticationBuilder builder);
}