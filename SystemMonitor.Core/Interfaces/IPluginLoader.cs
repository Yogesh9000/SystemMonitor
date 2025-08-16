namespace SystemMonitor.Core.Interfaces;

/// <summary>
/// Defines a contract for loading and managing system monitor plugins.
/// </summary>
public interface IPluginLoader
{
    /// <summary>
    /// Gets the collection of <see cref="ISystemMonitorPlugin"/> instances 
    /// that have been discovered and loaded by the plugin loader.
    /// </summary>
    List<ISystemMonitorPlugin> LoadedSystemMonitorPlugins { get; }
}