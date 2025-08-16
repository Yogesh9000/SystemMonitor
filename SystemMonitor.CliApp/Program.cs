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
            .AddCommandLine(args) // arguments can also be overriden from json
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

        // Get names of enabled plugins from both cli and json and merge them
        var jsonPlugins = configuration.GetSection("Plugins:Enabled").Get<List<string>>() ?? [];
        var cliPlugins = args
            .Where(a => a.StartsWith("EnablePlugin", StringComparison.OrdinalIgnoreCase))
            .Select(a => a.Split('=')[1]) // grab the RHS value
            .ToList();
        List<string> enabledPlugins = cliPlugins.Count > 0 ? cliPlugins : jsonPlugins;
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
        var delay = configuration.GetValue("Delay", 1000); // Get delay from config
        await monitoringService.Run(delay);
        logger.LogInformation("ShutDown Application Successfully");
    }
}