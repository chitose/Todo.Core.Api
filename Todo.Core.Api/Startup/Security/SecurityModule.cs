using Autofac;
using Todo.Core.Common.Startup;

namespace Todo.Core.Api.Startup.Security;

public class SecurityModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<GoogleAuth>().As<IExternalAuthenticationConfiguration>();
        builder.RegisterType<GoogleClaimToUserTransformer>()
            .Named<IExternalClaimsToUserTransformer>("google");
        
        builder.RegisterType<TwitterAuth>().As<IExternalAuthenticationConfiguration>();
        builder.RegisterType<TwitterClaimToUserTransformer>()
            .Named<IExternalClaimsToUserTransformer>("twitter");
    }
}