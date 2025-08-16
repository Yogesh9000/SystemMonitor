using Microsoft.Extensions.Configuration;
using Moq;
using SystemMonitor.Infrastructure.Plugins;

namespace SystemMonitor.Infrastructure.Tests.Plugins;

public class SystemMonitorPluginConfigTests
{
    [Fact]
    public void GetConfigValue_WhenCalledWithValidConfigValue_ShouldReturnConfigValue()
    {
        // Arrange
        var config = new Mock<IConfiguration>();
        config.Setup(c => c["key"]).Returns("value");
        var pluginConfig = new SystemMonitorPluginConfig(config.Object);
        
        // Act
        var value  = pluginConfig.GetConfigValue("key");
        
        // Assert
        Assert.NotNull(value);
        Assert.Equal("value", value);
    }
}