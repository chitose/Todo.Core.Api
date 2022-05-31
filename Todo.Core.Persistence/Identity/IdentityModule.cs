using Autofac;
using Microsoft.AspNetCore.Identity;
using NHibernate;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Identity;

public class IdentityModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<TodoUserStore>().As<IUserStore<User>>().InstancePerLifetimeScope();
        builder.RegisterType<TodoUserManager>().As<UserManager<User>>().InstancePerLifetimeScope();
        builder.RegisterType<TodoSigninManager>().As<SignInManager<User>>().InstancePerLifetimeScope();
        builder.RegisterType<PasswordHasher<User>>().As<IPasswordHasher<User>>().InstancePerLifetimeScope();
        builder.RegisterType<UpperInvariantLookupNormalizer>().As<ILookupNormalizer>().InstancePerLifetimeScope();
        builder.RegisterType<IdentityErrorDescriber>().AsSelf().InstancePerLifetimeScope();
        builder.Register(c => c.Resolve<ISessionFactory>().WithOptions().OpenSession()).As<ISession>()
            .InstancePerLifetimeScope();
    }
}