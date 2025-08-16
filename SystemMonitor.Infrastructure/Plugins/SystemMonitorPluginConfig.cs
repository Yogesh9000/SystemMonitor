using Microsoft.Extensions.Configuration;
using SystemMonitor.Core.Interfaces;

namespace SystemMonitor.Infrastructure.Plugins;

public class SystemMonitorPluginConfig(IConfiguration configuration) : ISystemMonitorPluginConfig
{
    public string? GetConfigValue(string key)
    {
        return configuration[key];
    }
}