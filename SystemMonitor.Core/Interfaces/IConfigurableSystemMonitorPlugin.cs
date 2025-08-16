namespace SystemMonitor.Core.Interfaces;

/// <summary>
/// Contract for Configurable System Monitor Plugins
/// </summary>
public interface IConfigurableSystemMonitorPlugin : ISystemMonitorPlugin
{
    /// <summary>
    /// Configure the plugin from <see cref="ISystemMonitorPluginConfig"/>
    /// </summary>
    /// <param name="systemMonitorPlugin"></param>
    public void Configure(ISystemMonitorPluginConfig systemMonitorPlugin);
}