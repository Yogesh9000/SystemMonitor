namespace SystemMonitor.Core.Interfaces;

public interface IPluginLoader
{
    List<ISystemMonitorPlugin> LoadedSystemMonitorPlugins { get; }
}