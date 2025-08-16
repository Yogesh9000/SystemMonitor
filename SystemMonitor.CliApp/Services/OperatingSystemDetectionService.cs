using SystemMonitor.CliApp.Models.Enum;

namespace SystemMonitor.CliApp.Services;

/// <summary>
/// Service for detecting operating system
/// Service is DI injectable
/// </summary>
public class OperatingSystemDetectionService
{
    public OperatingSystemType GetCurrentOperatingSystem()
    {
        if (OperatingSystem.IsWindows())
        {
            return OperatingSystemType.Windows;
        }

        if (OperatingSystem.IsLinux())
        {
            return OperatingSystemType.Unix;
        }

        if (OperatingSystem.IsMacOS())
        {
            return OperatingSystemType.MacOs;
        }
        return OperatingSystemType.Other;
    }
}