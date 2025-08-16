using System.Text;
using Microsoft.Extensions.Logging;
using SystemMonitor.Core.Interfaces;
using SystemMonitor.Core.Models;
using SystemMonitor.Infrastructure.Extensions;

namespace SystemMonitor.Plugin.LogToFile;

/// <summary>
/// A SystemMonitor plugin that logs system resource usage to a file.
/// Defaults to <c>{AppContext.BaseDirectory}\log.txt</c> unless overridden by 
/// the <c>LogToFile:FilePath</c> configuration key.
/// Creates the file and any missing directories if they do not exist.
/// Appends new log entries without overwriting existing content.
/// </summary>
public class LogToFilePlugin(ILogger<LogToFilePlugin> logger) : IConfigurableSystemMonitorPlugin
{
    private string _logFilePath = Path.Combine(AppContext.BaseDirectory, "log.txt");
    private readonly ILogger _logger = logger;

    public string Name { get; } = "LogToFile";
    public string Description { get; } = "Plugin to log system resource usage data to file";

    public Task OnSystemResourceUsageDataReceived(SystemResourceUsageDto systemResourceUsage)
    {
        StringBuilder sb = new();
        _ =sb.AppendLine("------------------------------------------------------------")
            .AppendLine($"Cpu Usage: {systemResourceUsage.CpuUsage.Used:0.00}%")
            .AppendLine($"Ram Total: {systemResourceUsage.RamUsage.Total.ToGb()}")
            .AppendLine($"Ram Used: {systemResourceUsage.RamUsage.Used.ToGb()}")
            .AppendLine("Disk Usage:");
        foreach (var diskUsage in systemResourceUsage.DiskUsage)
        {
            _ = sb.Append($"  Name: {diskUsage.Name}, Used: {diskUsage.Used.ToGb()}, Total: {diskUsage.Total.ToGb()}");
        }
        _ = sb.AppendLine();
        LogToFilePath(_logFilePath, sb.ToString());
        return Task.CompletedTask;
    }

    /// <summary>
    /// Configure the plugin
    /// </summary>
    /// <param name="config"></param>
    public void Configure(ISystemMonitorPluginConfig config)
    {
        _logFilePath = config.GetConfigValue("LogToFile:FilePath") ?? _logFilePath;
    }

    /// <summary>
    /// Logs msg to file.
    /// Creates file and necessary directories, if they do not exist
    /// </summary>
    /// <param name="logFilePath"></param>
    /// <param name="msg"></param>
    private void LogToFilePath(string logFilePath, string msg)
    {
        var logFileDir = Path.GetDirectoryName(_logFilePath);
        if (logFileDir is not null && !Directory.Exists(logFileDir))
        {
            Directory.CreateDirectory(logFileDir);
        }
        File.AppendAllText(logFilePath, msg);
    }
}