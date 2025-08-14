namespace SystemMonitor.Core.Models;

/// <summary>
/// Data transfer object for System Resource Usage
/// </summary>
public record SystemResourceUsageDto(CpuUsageDto CpuUsage, RamUsageDto RamUsage, List<DiskUsageDto> DiskUsage);