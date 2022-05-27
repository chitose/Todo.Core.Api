using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Todo.Core.Common.Context;

namespace Todo.Core.Common.Autofac;

public static class WebApplicationBuilderExtensions
{
    public static IContainer UseAutofac(this WebApplicationBuilder builder, IList<string> asembliesFilters)
    {
        var factory = new AutofacServiceProviderFactory(cfg =>
        {
            var rootDir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            foreach (var filter in asembliesFilters)
            {
                var assemblies = rootDir.GetFiles(filter).Select(f => Assembly.LoadFrom(f.FullName))
                    .ToArray();
                cfg.RegisterAssemblyModules(assemblies);
                foreach (var assembly in assemblies) cfg.RegisterAutoMapper(assembly);
            }
        });
        builder.Host.UseServiceProviderFactory(factory);
        var container = factory.CreateBuilder(builder.Services).Build();
        var contextHandler = container.Resolve<IContextHandler>();
        UserContext.InitializeContext(contextHandler);

        return container;
    }
}