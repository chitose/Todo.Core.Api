using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Todo.Core.Common.Context;
using Todo.Core.Common.Startup;

namespace Todo.Core.Common.Autofac;

public static class WebApplicationBuilderExtensions
{
    public static IContainer UseAutofac(this WebApplicationBuilder builder, IList<string> asembliesFilters)
    {
        var factory = new AutofacServiceProviderFactory(cfg =>
        {
            var startupConfig = new List<Type>();
            var rootDir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            foreach (var filter in asembliesFilters)
            {
                var assemblies = rootDir.GetFiles(filter).Select(f => Assembly.LoadFrom(f.FullName))
                    .ToArray();
                cfg.RegisterAssemblyModules(assemblies);
                foreach (var assembly in assemblies)
                {
                    cfg.RegisterAutoMapper(assembly);
                    var startups = assembly.GetTypes().Where(t =>
                        typeof(IStartupConfiguration).IsAssignableFrom(t)
                        && t.IsClass);
                    startupConfig.AddRange(startups);
                }
            }

            var startupInstances = startupConfig.Select(s => Activator.CreateInstance(s) as IStartupConfiguration)
                .OrderBy(x => x.Order);
            foreach (var s in startupInstances) s.ConfigureBuilder(builder);
        });
        builder.Host.UseServiceProviderFactory(factory);
        var container = factory.CreateBuilder(builder.Services).Build();
        var contextHandler = container.Resolve<IContextHandler>();
        UserContext.InitializeContext(contextHandler);

        return container;
    }
}