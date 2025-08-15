namespace SystemMonitor.Core.Models;

/// <summary>
/// Data transfer object for system Ram Usage
/// </summary>
public record RamUsageDto(Memory Used, Memory Total);