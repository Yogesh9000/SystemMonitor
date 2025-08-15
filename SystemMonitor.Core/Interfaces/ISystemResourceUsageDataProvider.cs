using SystemMonitor.Core.Models;

namespace SystemMonitor.Core.Interfaces;

public interface ISystemResourceUsageDataProvider
{
    public SystemResourceUsageDto  GetSystemResourceUsage();
}