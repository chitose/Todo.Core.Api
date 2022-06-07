using System.ComponentModel;

namespace Todo.Core.Common.Configuration;

public class JsonConfigProvider : IConfigProvider
{
    private readonly IConfiguration _configurationRoot;

    public JsonConfigProvider()
    {
        var configBuilder = new ConfigurationBuilder();
        configBuilder.AddJsonFile("appsettings.json");
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (!string.IsNullOrEmpty(environment))
        {
            configBuilder.AddJsonFile($"appSettings.{environment}.json", true);
        }

        if (environment == "Development")
        {
            configBuilder.AddJsonFile("appsettings.private.json", true);
        }

        _configurationRoot = configBuilder.Build();
    }

    public T? GetConfigValue<T>(string key)
    {
        var strValue = GetConfigValue(key);
        var converter = TypeDescriptor.GetConverter(typeof(T));
        if (converter == null) throw new NotSupportedException($"No supported converter for type {typeof(T)}");

        return (T?) converter.ConvertFromString(strValue);
    }

    public string GetConfigValue(string key)
    {
        return _configurationRoot[key];
    }

    public string GetConnectionString(string key)
    {
        return _configurationRoot.GetConnectionString(key);
    }

    public T GetSection<T>(string sectionName) where T : new()
    {
        var settings = new T();
        _configurationRoot.GetSection(sectionName).Bind(settings);
        return settings;
    }

    public void Bind(string section, object configInstance)
    {
        _configurationRoot.GetSection(section).Bind(configInstance);
    }
}