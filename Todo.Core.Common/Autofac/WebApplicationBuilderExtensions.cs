using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;

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
                var assemblies = rootDir.GetFiles(filter).Select(f => Assembly.LoadFrom(f.FullName));
                cfg.RegisterAssemblyModules(assemblies.ToArray());
            }
        });
        builder.Host.UseServiceProviderFactory(factory);
        return factory.CreateBuilder(builder.Services).Build();
    }
}