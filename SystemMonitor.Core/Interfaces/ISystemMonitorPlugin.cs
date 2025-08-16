using SystemMonitor.Core.Models;

namespace SystemMonitor.Core.Interfaces;

/// <summary>
/// Contract for System Monitor Plugins
/// </summary>
public interface ISystemMonitorPlugin
{
    /// <summary>
    /// Plugin Name
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Description
    /// </summary>
    public string Description { get; }
    
    /// <summary>
    /// Process System Resource Usage Data 
    /// </summary>
    /// <param name="systemResourceUsage"></param>
    /// <returns></returns>
    public Task OnSystemResourceUsageDataReceived(SystemResourceUsageDto systemResourceUsage);
}