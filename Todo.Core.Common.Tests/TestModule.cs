using Autofac;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Todo.Core.Common.Tests;

public class TestModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<HttpAccessorMock>().As<IHttpContextAccessor>();
        builder.RegisterType<AutenticationSchemeProviderMock>().As<IAuthenticationSchemeProvider>();
    }
}