using SystemMonitor.Core.Models;

namespace SystemMonitor.Core.Interfaces;

public interface ISystemMonitorPlugin
{
    public void OnSystemResourceUsageDataReceived(SystemResourceUsageDto systemResourceUsage);
}