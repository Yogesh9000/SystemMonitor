namespace SystemMonitor.Core.Interfaces;

public interface ISystemMonitorPluginConfig
{
    public string? GetConfigValue(string key);
}