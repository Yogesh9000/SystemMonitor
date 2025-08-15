using SystemMonitor.Core.Interfaces;
using SystemMonitor.Core.Models;

namespace SystemMonitor.Infrastructure.Unix;

public class UnixSystemResourceUsageDataProvider : ISystemResourceUsageDataProvider
{
    public SystemResourceUsageDto GetSystemResourceUsage()
    {
        throw new NotImplementedException();
    }
}