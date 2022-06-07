using System.Reflection;
using Microsoft.OpenApi.Models;
using Todo.Core.Common.Startup;

namespace Todo.Core.Api.Startup;

public class SwaggerStartupConfiguration : IBuilderStartupConfiguration, IAppStartupConfiguration
{
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "ToDo API",
                Description = "An .Netcore Web API for Todo application",
            });
            
            // using System.Reflection;
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
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