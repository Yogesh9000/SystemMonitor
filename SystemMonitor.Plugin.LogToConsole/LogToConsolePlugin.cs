using SystemMonitor.Core.Interfaces;
using SystemMonitor.Core.Models;
using SystemMonitor.Infrastructure.Extensions;

namespace SystemMonitor.Plugin.LogToConsole;

/// <summary>
/// Plugin to log system resource usage to console
/// </summary>
public class LogToConsolePlugin : ISystemMonitorPlugin
{
    public string Name { get; } = "LogToConsole";
    public string Description { get; } = "Plugin to log system resource usage data to console";

    /// <summary>
    /// Log the system resource usage to console
    /// </summary>
    /// <param name="systemResourceUsage"></param>
    /// <returns></returns>
    public Task OnSystemResourceUsageDataReceived(SystemResourceUsageDto systemResourceUsage)
    {
        Console.WriteLine("------------------------------------------------------------");
        Console.WriteLine($"Cpu Usage: {systemResourceUsage.CpuUsage.Used:0.00}%");
        Console.WriteLine($"Ram Total: {systemResourceUsage.RamUsage.Total.ToGb()}");
        Console.WriteLine($"Ram Used: {systemResourceUsage.RamUsage.Used.ToGb()}");
        Console.WriteLine("Disk Usage:");
        foreach (var diskUsage in systemResourceUsage.DiskUsage)
        {
            Console.WriteLine($"  Name: {diskUsage.Name}, Used: {diskUsage.Used.ToGb()}, Total: {diskUsage.Total.ToGb()}");
        }
        return Task.CompletedTask;
    }
}