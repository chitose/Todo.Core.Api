using Todo.Core.Common.Startup;

namespace Todo.Core.Api.Startup;

public class DevConfigurationStartupConfiguration : IBuilderStartupConfiguration
{
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration.AddJsonFile("appsettings.private.json", true);
        }
    }
}