namespace Todo.Core.Common.Startup;

public interface IBuilderStartupConfiguration
{
    void ConfigureBuilder(WebApplicationBuilder builder);
}