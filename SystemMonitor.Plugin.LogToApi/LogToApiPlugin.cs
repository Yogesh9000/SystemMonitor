using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using SystemMonitor.Core.Interfaces;
using SystemMonitor.Core.Models;
using SystemMonitor.Plugin.LogToApi.Models;

namespace SystemMonitor.Plugin.LogToApi;

/// <summary>
/// A system monitor plugin that sends collected CPU, RAM, and disk usage data
/// to a remote API endpoint using HTTP POST.
/// </summary>
/// <remarks>
/// The API endpoint URL must be provided via the <c>ISystemMonitorPluginConfig</c> key:
/// <c>"LogToApi:EndPoint"</c>.  
/// If the key is missing or empty, the plugin will not attempt to send data.
/// </remarks>
public class LogToApiPlugin(HttpClient httpClient, ILogger<LogToApiPlugin> logger) : IConfigurableSystemMonitorPlugin
{
    public string Name { get; } = "LogToApi";
    public string Description { get; } = "Plugin to log system resource usage data to api endpoint";

    private string? _endpoint;

    public async Task OnSystemResourceUsageDataReceived(SystemResourceUsageDto systemResourceUsage)
    {
        if (_endpoint is null)
        {
            logger.Log(LogLevel.Information, "No valid endpoint to log data.");
        }

        // Setup payload message
        var cpuUsage = systemResourceUsage.CpuUsage.Used;
        var ramUsage = systemResourceUsage.RamUsage.Used.Size;
        var diskUsage = systemResourceUsage.DiskUsage.Select(d => d.Used.Size);
        ApiResourceUsagePayloadDto payload = new(cpuUsage, ramUsage, diskUsage.ToList());
        var payloadString = JsonSerializer.Serialize(payload);

        try
        {
            // Send payload message to endpoint
            var response = await httpClient.PostAsync(_endpoint,
                new StringContent(payloadString, Encoding.UTF8, "application/json"));
        
            // log message if we failed to send the payload to endpoint
            if (!response.IsSuccessStatusCode)
            {
                logger.Log(LogLevel.Error, $"Error occurred while logging data to endpoint: {_endpoint}.");
            }
        }
        catch (Exception e)
        {
            logger.Log(LogLevel.Error, $"Error occurred while logging data to endpoint: {_endpoint}, with error message: {e.Message}.");
        }
    }

    public void Configure(ISystemMonitorPluginConfig config)
    {
        _endpoint = config.GetConfigValue("LogToApi:EndPoint");
    }
}