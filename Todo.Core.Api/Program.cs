using Autofac;
using Todo.Core.Common.Autofac;
using Todo.Core.Common.Startup;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets("793431f5-9957-447d-87be-1e455b735d58");

var ar = builder.UseAutofac(new[]
{
    "Todo.Core.*.dll"
});

WebApplication app;

using (var scope = ar.BeginLifetimeScope())
{
    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    app = builder.Build();
    var startupConfigs = scope.Resolve<IEnumerable<IStartupConfiguration>>();
    foreach (var sc in startupConfigs) sc.ConfigureApp(app);

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
}

app.Run();