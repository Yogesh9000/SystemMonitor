namespace SystemMonitor.Core.Models;

/// <summary>
/// Data transfer object for system Disk Usage
/// </summary>
public record DiskUsageDto(string Name, double Used, double Total);