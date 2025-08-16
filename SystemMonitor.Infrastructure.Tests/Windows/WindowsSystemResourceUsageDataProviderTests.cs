using System.Runtime.Versioning;
using SystemMonitor.Infrastructure.Windows;

namespace SystemMonitor.Infrastructure.Tests.Windows;

public class WindowsSystemResourceUsageDataProviderTests
{
    [SupportedOSPlatform("windows")]
    [Fact]
    public void GetSystemResourceUsage_WhenCalled_ReturnsSystemResourceUsageData()
    {
        // Arrange
        var usageProvider = new WindowsSystemResourceUsageDataProvider();
        
        // Act
        var usageData = usageProvider.GetSystemResourceUsage();
        
        // Assert
        Assert.NotNull(usageData);
    }
}