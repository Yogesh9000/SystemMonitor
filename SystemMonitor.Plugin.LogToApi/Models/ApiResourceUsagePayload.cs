using System.Text.Json.Serialization;

namespace SystemMonitor.Plugin.LogToApi.Models;

/// <summary>
/// Data transfer object for resource usage payload
/// </summary>
public record ApiResourceUsagePayloadDto(double CpuUsed, double RamUsed, List<double> DiskUsed)
{
    [JsonPropertyName("cpu")] public double CpuUsed { get; set; } = CpuUsed;

    [JsonPropertyName("ram_used")] public double RamUsed { get; set; } = RamUsed;

    [JsonPropertyName("disk_used")] public List<double> DiskUsed { get; set; } = DiskUsed;
}