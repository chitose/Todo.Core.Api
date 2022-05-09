namespace Todo.Core.Common.Startup;

public interface IStartupConfiguration
{
    int Order { get; }

    void ConfigureBuilder(WebApplicationBuilder builder);

    void ConfigureApp(WebApplication app);
}