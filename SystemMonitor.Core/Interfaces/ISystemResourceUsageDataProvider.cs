using SystemMonitor.Core.Models;

namespace SystemMonitor.Core.Interfaces;

/// <summary>
/// Contract for classes providing <see cref="SystemResourceUsageDto"/>
/// </summary>
public interface ISystemResourceUsageDataProvider
{
    public SystemResourceUsageDto GetSystemResourceUsage();
}