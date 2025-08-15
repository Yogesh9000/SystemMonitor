using SystemMonitor.Core.Interfaces;
using SystemMonitor.Core.Models;
using SystemMonitor.Infrastructure.Extensions;

namespace SystemMonitor.Plugin.LogToConsole;

public class LogToConsolePlugin : ISystemMonitorPlugin
{
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