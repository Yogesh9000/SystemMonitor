using SystemMonitor.Core.Models;

namespace SystemMonitor.Core.Interfaces;

public interface ISystemMonitorPlugin
{
    public Task OnSystemResourceUsageDataReceived(SystemResourceUsageDto systemResourceUsage);
}