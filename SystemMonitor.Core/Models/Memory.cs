using SystemMonitor.Core.Models.Enum;

namespace SystemMonitor.Core.Models;

/// <summary>
/// Object to represent memory space in <see cref="MemoryUnit"/>
/// </summary>
/// <param name="Size"></param>
/// <param name="Unit"></param>
public record Memory(double Size, MemoryUnit Unit);