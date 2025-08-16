using Microsoft.Extensions.Configuration;
using SystemMonitor.Core.Interfaces;

namespace SystemMonitor.Infrastructure.Plugins;

/// <summary>
/// Config provider for system monitor plugins
/// </summary>
/// <param name="configuration"></param>
public class SystemMonitorPluginConfig(IConfiguration configuration) : ISystemMonitorPluginConfig
{
    /// <summary>
    /// Get config value from key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string? GetConfigValue(string key)
    {
        return configuration[key];
    }
}