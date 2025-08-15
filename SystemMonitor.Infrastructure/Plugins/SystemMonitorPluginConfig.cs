using SystemMonitor.Core.Interfaces;

namespace SystemMonitor.Infrastructure.Plugins;

public class SystemMonitorPluginConfig(Dictionary<string, string> config) : ISystemMonitorPluginConfig
{
    public string? GetConfigValue(string key)
    {
        return config.GetValueOrDefault(key);
    }
}