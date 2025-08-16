namespace SystemMonitor.Core.Interfaces;

/// <summary>
/// Contract for class defining plugin config
/// </summary>
public interface ISystemMonitorPluginConfig
{
    /// <summary>
    /// Get config value from key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string? GetConfigValue(string key);
}