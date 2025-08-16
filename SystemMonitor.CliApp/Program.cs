using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SystemMonitor.CliApp.Models.Enum;
using SystemMonitor.CliApp.Services;
using SystemMonitor.Core.Interfaces;
using SystemMonitor.Infrastructure.Plugins;
using SystemMonitor.Infrastructure.Unix;
using SystemMonitor.Infrastructure.Windows;

namespace SystemMonitor.CliApp;

class Program
{
    public static async Task Main(string[] args)
    {
        // Load configuration from appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory) // ensures correct runtime path
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Setup DI container
        var serviceCollection = new ServiceCollection()
            .AddLogging(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Information);
            })
            .AddSingleton<IConfiguration>(configuration)
            .AddSingleton<ISystemMonitorPluginConfig, SystemMonitorPluginConfig>()
            .AddSingleton<DirectoryPluginLoader>()
            .AddSingleton<SystemResourceUsageDataProviderResolver>()
            .AddKeyedSingleton<ISystemResourceUsageDataProvider, UnixSystemResourceUsageDataProvider>(
                OperatingSystemType.Unix)
            .AddSingleton<OperatingSystemDetectionService>()
            .AddSingleton<SystemResourceUsageDataProviderResolver>()
            .AddSingleton(new HttpClient())
            .AddSingleton<SystemResourceUsageMonitoringService>();
        if (OperatingSystem.IsWindows())
        {
            // Only register this if we are on windows, otherwise the compiler complains
            serviceCollection
                .AddKeyedSingleton<ISystemResourceUsageDataProvider, WindowsSystemResourceUsageDataProvider>(
                    OperatingSystemType.Windows);
        }

        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

        // Get names of enabled plugins
        List<string> enabledPlugins = configuration.GetSection("Plugins:Enabled").Get<List<string>>() ?? [];
        var path = configuration["Plugins:Path"];
        if (string.IsNullOrEmpty(path))
        {
            logger.LogInformation("Plugin Path is empty");
            return;
        }
        // Get the absolute path of plugin directory
        var fullPath = Path.IsPathRooted(path) ? path : Path.Combine(AppContext.BaseDirectory, path);
        
        // Create monitoring service 
        var monitoringService = serviceProvider.GetRequiredService<SystemResourceUsageMonitoringService>();
        // load plugins
        monitoringService.LoadPluginsFromDirectory(fullPath, enabledPlugins);
        // begin monitoring
        await monitoringService.Run(1000);
        logger.LogInformation("ShutDown Application Successfully");
    }
}