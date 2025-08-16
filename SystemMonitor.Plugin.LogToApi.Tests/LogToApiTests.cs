using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using SystemMonitor.Core.Interfaces;
using SystemMonitor.Core.Models;
using SystemMonitor.Core.Models.Enum;
using SystemMonitor.Plugin.LogToApi.Models;

namespace SystemMonitor.Plugin.LogToApi.Tests;

public class LogToApiTests
{
    [Fact]
    public async Task  OnSystemResourceUsageDataReceived_WhenCalledWithValidUsageData_ShouldSendItToConfiguredEndPoint()
    {
                // Arrange
        var endpoint = "http://localhost/test-endpoint";

        // Mock HttpMessageHandler to intercept requests
        var handlerMock = new Mock<HttpMessageHandler>();
        HttpRequestMessage? capturedRequest = null;

        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync((HttpRequestMessage request, CancellationToken _) =>
            {
                capturedRequest = request;
                return new HttpResponseMessage(HttpStatusCode.OK);
            });

        var httpClient = new HttpClient(handlerMock.Object);

        var mockLogger = new Mock<ILogger<LogToApiPlugin>>();
        var plugin = new LogToApiPlugin(httpClient, mockLogger.Object);

        var mockConfig = new Mock<ISystemMonitorPluginConfig>();
        mockConfig.Setup(c => c.GetConfigValue("LogToApi:EndPoint"))
                  .Returns(endpoint);
        plugin.Configure(mockConfig.Object);

        var dto = new SystemResourceUsageDto(
            CpuUsage: new(Used: 1.0),
            RamUsage: new(Used: new(2.0, MemoryUnit.Bytes), Total: new(0.0, MemoryUnit.Bytes)),
            DiskUsage: []);

        // Act
        await plugin.OnSystemResourceUsageDataReceived(dto);

        // Assert
        Assert.NotNull(capturedRequest);
        Assert.Equal(HttpMethod.Post, capturedRequest!.Method);
        Assert.Equal(endpoint, capturedRequest.RequestUri!.ToString());
        Assert.Equal("application/json", capturedRequest.Content!.Headers.ContentType!.MediaType);

        var body = await capturedRequest.Content.ReadAsStringAsync();
        var payload = JsonSerializer.Deserialize<ApiResourceUsagePayloadDto>(body);

        Assert.NotNull(payload);
        Assert.Equal(1.0, payload.CpuUsed);
        Assert.Equal(2.0, payload.RamUsed);
        Assert.Empty(payload.DiskUsed);
    }
}