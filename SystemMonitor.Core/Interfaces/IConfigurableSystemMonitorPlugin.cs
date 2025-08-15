namespace SystemMonitor.Core.Interfaces;

public interface IConfigurableSystemMonitorPlugin : ISystemMonitorPlugin
{
    public void Configure(ISystemMonitorPluginConfig systemMonitorPlugin);
}