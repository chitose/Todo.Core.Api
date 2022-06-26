using Todo.Core.Common.Configuration;

namespace Todo.Core.Common.Extensions;

public static class ConfigProviderExtensions
{
    public static string GetConnectionType(this IConfigProvider configProvider)
    {
        return configProvider.GetConfigValue("ConnectionType");
    }
}