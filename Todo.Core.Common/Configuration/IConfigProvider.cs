namespace Todo.Core.Common.Configuration;

public interface IConfigProvider
{
    T? GetConfigValue<T>(string key);

    string GetConfigValue(string key);

    string GetConnectionString(string key);

    T GetSection<T>(string sectionName) where T : new();

    void Bind(string section, object configInstance);
}