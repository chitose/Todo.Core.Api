namespace Todo.Core.Common.Startup;

public interface IAppStartupConfiguration
{
    int Order { get; }
    void ConfigureApp(WebApplication app);
}