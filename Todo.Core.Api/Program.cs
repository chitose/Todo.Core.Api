using Autofac;
using Todo.Core.Common.Autofac;
using Todo.Core.Common.Startup;

var builder = WebApplication.CreateBuilder(args);

var ar = builder.UseAutofac(new[] { "Todo.Core.*.dll" });

WebApplication app = null;
using (var scope = ar.BeginLifetimeScope())
{
    var startupConfigs = scope.Resolve<IEnumerable<IStartupConfiguration>>().OrderBy(x => x.Order);

    foreach (var sc in startupConfigs) sc.ConfigureBuilder(builder);

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    app = builder.Build();

    foreach (var sc in startupConfigs) sc.ConfigureApp(app);

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();
}

app.Run();