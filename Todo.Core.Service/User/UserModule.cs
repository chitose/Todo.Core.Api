using Autofac;

namespace Todo.Core.Service.User;

public class UserModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<UserService>().As<IUserService>();
    }
}