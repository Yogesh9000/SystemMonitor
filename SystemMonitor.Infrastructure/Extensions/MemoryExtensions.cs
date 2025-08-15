using SystemMonitor.Core.Models;
using SystemMonitor.Core.Models.Enum;

namespace SystemMonitor.Infrastructure.Extensions;

public static class MemoryExtensions
{
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