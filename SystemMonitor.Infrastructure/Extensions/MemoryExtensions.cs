using SystemMonitor.Core.Models;
using SystemMonitor.Core.Models.Enum;

namespace SystemMonitor.Infrastructure.Extensions;

/// <summary>
/// Extension methods for <see cref="Memory"/>
/// </summary>
public static class MemoryExtensions
{
    /// <summary>
    /// Convert Memory to Bytes
    /// </summary>
    /// <param name="memory"></param>
    /// <returns></returns>
    public static Memory ToBytes(this Memory memory)
    {
        return memory.Unit switch
        {
            MemoryUnit.Kilobytes => new Memory(memory.Size * 1024, MemoryUnit.Bytes),
            MemoryUnit.Megabytes => new Memory(memory.Size * 1024 * 1024, MemoryUnit.Bytes),
            MemoryUnit.Gigabytes => new Memory(memory.Size * 1024 * 1024 * 1024, MemoryUnit.Bytes),
            _ => memory
        };
    }
    
    /// <summary>
    /// Convert Memory to KiloBytes
    /// </summary>
    /// <param name="memory"></param>
    /// <returns></returns>
    public static Memory ToKb(this Memory memory)
    {
        return memory.Unit switch
        {
            MemoryUnit.Bytes => new Memory(memory.Size / 1024, MemoryUnit.Kilobytes),
            MemoryUnit.Megabytes => new Memory(memory.Size * 1024, MemoryUnit.Kilobytes),
            MemoryUnit.Gigabytes => new Memory(memory.Size * 1024 * 1024, MemoryUnit.Kilobytes),
            _ => memory
        };
    }
    
    /// <summary>
    /// Convert Memory to MegaBytes
    /// </summary>
    /// <param name="memory"></param>
    /// <returns></returns>
    public static Memory ToMb(this Memory memory)
    {
        return memory.Unit switch
        {
            MemoryUnit.Bytes => new Memory(memory.Size / (1024 * 1024), MemoryUnit.Megabytes),
            MemoryUnit.Kilobytes => new Memory(memory.Size / 1024, MemoryUnit.Megabytes),
            MemoryUnit.Gigabytes => new Memory(memory.Size * 1024, MemoryUnit.Megabytes),
            _ => memory
        };
    }
    
    /// <summary>
    /// Convert Memory to GigaBytes
    /// </summary>
    /// <param name="memory"></param>
    /// <returns></returns>
    public static Memory ToGb(this Memory memory)
    {
        return memory.Unit switch
        {
            MemoryUnit.Bytes => new Memory(memory.Size / (1024 * 1024 * 1024), MemoryUnit.Gigabytes),
            MemoryUnit.Kilobytes => new Memory(memory.Size / (1024 * 1024), MemoryUnit.Gigabytes),
            MemoryUnit.Megabytes => new Memory(memory.Size / 1024, MemoryUnit.Gigabytes),
            _ => memory
        };
    }
}