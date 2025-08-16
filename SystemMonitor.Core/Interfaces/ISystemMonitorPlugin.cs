using SystemMonitor.Core.Models;

namespace SystemMonitor.Core.Interfaces;

public interface ISystemMonitorPlugin
{
    public string Name { get; }
    public string Description { get; }
    public Task OnSystemResourceUsageDataReceived(SystemResourceUsageDto systemResourceUsage);
}