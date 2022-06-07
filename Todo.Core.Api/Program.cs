using Todo.Core.Common.Autofac;

var builder = WebApplication.CreateBuilder(args);

var ar = builder.UseAutofac(new[] {"Todo.Core.*.dll"});

WebApplication app;

using (var scope = ar.BeginLifetimeScope())
{
    app = builder.Build();
    app.ConfigureStartup(scope);
}

app.Run();