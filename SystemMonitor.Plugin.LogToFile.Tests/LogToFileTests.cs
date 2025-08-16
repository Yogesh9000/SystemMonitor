using Microsoft.Extensions.Logging;
using SystemMonitor.Core.Interfaces;
using SystemMonitor.Core.Models;
using Moq;
using SystemMonitor.Core.Models.Enum;

namespace SystemMonitor.Plugin.LogToFile.Tests;

public class LogToFileTests
{
    [Fact]
    public async Task OnSystemResourceUsageDataReceived_WhenCalledWithValidUsageData_ShouldWriteItToLogFile()
    {
        // Arrange
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);

        var logFilePath = Path.Combine(tempDir, "log.txt");

        var mockLogger = new Mock<ILogger<LogToFilePlugin>>();
        var plugin = new LogToFilePlugin(mockLogger.Object);

        // Configure the plugin to use our test file path
        var mockConfig = new Mock<ISystemMonitorPluginConfig>();
        mockConfig.Setup(c => c.GetConfigValue("LogToFile:FilePath"))
            .Returns(logFilePath);
        plugin.Configure(mockConfig.Object);

        var dto = new SystemResourceUsageDto(
            CpuUsage: new (Used: 0.0),
            RamUsage: new(Used: new (0.0, MemoryUnit.Bytes), Total: new (0.0, MemoryUnit.Bytes)),
            DiskUsage: []);
        

        // Act
        await plugin.OnSystemResourceUsageDataReceived(dto);

        // Assert
        Assert.True(File.Exists(logFilePath), "Log file should be created");
        var fileContent = File.ReadAllText(logFilePath);
        Assert.False(string.IsNullOrWhiteSpace(fileContent), "Log file should not be empty");

        // Cleanup
        Directory.Delete(tempDir, true);
    }
}