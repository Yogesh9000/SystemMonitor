using SystemMonitor.Core.Models.Enum;

namespace SystemMonitor.Core.Models;

/// <summary>
/// Object to represent memory space in <see cref="MemoryUnit"/>
/// </summary>
/// <param name="Size"></param>
/// <param name="Unit"></param>
public record Memory(double Size, MemoryUnit Unit)
{
    /// <summary>
    /// Pretty print memory with units
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        var unit = Unit switch
        {
            MemoryUnit.Bytes => "B",
            MemoryUnit.Kilobytes => "KB",
            MemoryUnit.Megabytes => "MB",
            MemoryUnit.Gigabytes => "GB",
            _ => ""
        };
        return $"{Size:0.00}{unit}";
    }
};