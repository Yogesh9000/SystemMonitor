using System.Diagnostics;
using System.Runtime.Versioning;
using SystemMonitor.Core.Interfaces;
using SystemMonitor.Core.Models;
using SystemMonitor.Core.Models.Enum;

namespace SystemMonitor.Infrastructure.Windows;

/// <summary>
/// Provides System Resource Usage Data (Cpu, Ram, Disk) on Windows
/// </summary>
[SupportedOSPlatform("windows")]
public sealed class WindowsSystemResourceUsageDataProvider : ISystemResourceUsageDataProvider
{
    private readonly PerformanceCounter _cpuPerformanceCounter;
    private readonly Process _currentProcess;

    /// <summary>
    /// Ctor
    /// </summary>
    public WindowsSystemResourceUsageDataProvider()
    {
        // Create new performance counter to return Cpu performance in percentage
        _cpuPerformanceCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

        _currentProcess = Process.GetCurrentProcess();

        // Warm Up Performance Counter, As First call always reads 0
        _cpuPerformanceCounter.NextValue();
    }

    /// <summary>
    /// Get System Resource Usage Data
    /// </summary>
    /// <returns>SystemResourceUsageDto</returns>
    /// <remarks>If there is not enough delay between two calls of this method, it might return garbage data</remarks>
    public SystemResourceUsageDto GetSystemResourceUsage() =>
        new SystemResourceUsageDto(CpuUsage: GetCpuUsage(), RamUsage: GetRamUsage(),
            DiskUsage: GetDiskUsage());

    /// <summary>
    /// Get the Cpu Usage
    /// </summary>
    /// <returns>CpuUsageDto</returns>
    /// <remarks>If there is not enough delay between two calls of this method, it might return garbage data</remarks>
    private CpuUsageDto GetCpuUsage() =>
        new(Used: _cpuPerformanceCounter.NextValue());

    /// <summary>
    /// Get the Ram Usage
    /// </summary>
    /// <returns>RamUsageDto</returns>
    private RamUsageDto GetRamUsage() =>
        new(Used: new Memory(Size: _currentProcess.WorkingSet64, MemoryUnit.Bytes),
            Total: new Memory(Size: GC.GetGCMemoryInfo().TotalAvailableMemoryBytes, MemoryUnit.Bytes));

    /// <summary>
    /// Get the Disk Usage
    /// </summary>
    /// <returns>DiskUsageDto's</returns>
    private List<DiskUsageDto> GetDiskUsage()
    {
        List<DiskUsageDto> diskUsageDtos = [];
        foreach (var driveInfo in DriveInfo.GetDrives())
        {
            if (driveInfo.IsReady)
            {
                DiskUsageDto diskUsageDto = new(Name: driveInfo.Name,
                    Used: new Memory(Size: driveInfo.AvailableFreeSpace, Unit: MemoryUnit.Bytes),
                    Total: new Memory(Size: driveInfo.TotalSize, Unit: MemoryUnit.Bytes));
                diskUsageDtos.Add(diskUsageDto);
            }
        }

        return diskUsageDtos;
    }
}