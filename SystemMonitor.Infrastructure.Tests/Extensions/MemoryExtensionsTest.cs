using SystemMonitor.Core.Models;
using SystemMonitor.Core.Models.Enum;
using SystemMonitor.Infrastructure.Extensions;

namespace SystemMonitor.Infrastructure.Tests.Extensions;

public class MemoryExtensionsTest
{
    // Arrange
    private readonly Memory _memory = new Memory(Size: 1024, Unit: MemoryUnit.Bytes);
    
    [Fact]
    public void ToBytes_WhenCalledWithBytes_ShouldReturnSameResultBack()
    {
        // Act
        var memory = _memory.ToBytes();
        
        // Assert
        Assert.Equal(_memory.Size, memory.Size);
        Assert.Equal(_memory.Unit, memory.Unit);
    }
    
    [Fact]
    public void ToKb_WhenCalledWithBytes_ShouldReturnMemoryAsKiloBytes()
    {
        // Act
        var memory = _memory.ToKb();
        
        // Assert
        Assert.Equal(_memory.Size / 1024, memory.Size);
        Assert.Equal(MemoryUnit.Kilobytes, memory.Unit);
    }
    
    [Fact]
    public void ToMb_WhenCalledWithBytes_ShouldReturnMemoryAsMegaBytes()
    {
        // Act
        var memory = _memory.ToMb();
        
        // Assert
        Assert.Equal(_memory.Size / (1024 * 1024), memory.Size);
        Assert.Equal(MemoryUnit.Megabytes, memory.Unit);
    }
    
    [Fact]
    public void ToGb_WhenCalledWithBytes_ShouldReturnMemoryAsGigaBytes()
    {
        // Act
        var memory = _memory.ToGb();
        
        // Assert
        Assert.Equal(_memory.Size / (1024 * 1024 * 1024), memory.Size);
        Assert.Equal(MemoryUnit.Gigabytes, memory.Unit);
    }
}