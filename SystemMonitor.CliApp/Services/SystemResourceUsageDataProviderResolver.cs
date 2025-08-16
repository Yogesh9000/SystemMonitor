using Microsoft.Extensions.DependencyInjection;
using SystemMonitor.CliApp.Models.Enum;
using SystemMonitor.Core.Interfaces;
using SystemMonitor.Infrastructure.Unix;
using SystemMonitor.Infrastructure.Windows;

namespace SystemMonitor.CliApp.Services;

public class SystemResourceUsageDataProviderResolver(IServiceProvider serviceProvider, OperatingSystemDetectionService operatingSystem)
{
    /// <summary>
    /// Get system resource usage data provider for current operatin system
    /// </summary>
    /// <returns></returns>
    public ISystemResourceUsageDataProvider? GetSystemResourceUsageDataProvider()
    {
        var currentOs = operatingSystem.GetCurrentOperatingSystem();
        return serviceProvider.GetKeyedService<ISystemResourceUsageDataProvider>(currentOs);
    }
}