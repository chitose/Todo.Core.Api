using Todo.Core.Common.Startup;

namespace Todo.Core.Api.Startup;

public class SwaggerStartupConfiguration : IBuilderStartupConfiguration, IAppStartupConfiguration
{
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }

    public int Order => StartupOrder.First;
    public void ConfigureApp(WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
        
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}