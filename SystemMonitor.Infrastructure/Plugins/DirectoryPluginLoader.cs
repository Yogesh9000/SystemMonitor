using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SystemMonitor.Core.Interfaces;

namespace SystemMonitor.Infrastructure.Plugins;

/// <summary>
/// Loads system monitor plugins from a specified directory at runtime.
/// </summary>
/// <remarks>
/// <para>
/// The loader scans the given directory for .NET assemblies (*.dll) and uses reflection to locate
/// types implementing <see cref="ISystemMonitorPlugin"/> or <see cref="IConfigurableSystemMonitorPlugin"/>. Each discovered type is instantiated
/// using its parameterless constructor.
/// </para>
/// <para>
/// All successfully created plugin instances are returned to the caller. Assemblies that cannot be
/// loaded, or types that do not match the plugin interface, are skipped.
/// </para>
/// <para>
/// Typical usage:
/// <code>
/// IPluginLoader loader = new DirectoryPluginLoader();
/// var plugins = loader.LoadPlugins("plugins");
/// foreach (var plugin in plugins)
/// {
///     plugin.OnDataReceived(data);
/// }
/// </code>
/// </para>
/// </remarks>
/// <example>
/// Example folder structure:
/// plugins/
///   MyPlugin.dll
///   AnotherPlugin.dll
/// </example>
public class DirectoryPluginLoader(IServiceProvider serviceProvider, ILogger<DirectoryPluginLoader> logger, ISystemMonitorPluginConfig config)
    : IPluginLoader
{
    public List<ISystemMonitorPlugin> LoadedSystemMonitorPlugins { get; } = [];

    public void LoadPluginsFromDirectory(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            logger.Log(LogLevel.Error, $"Directory not found: {directoryPath}");
            return;
        }

        foreach (var dllPath in Directory.GetFiles(directoryPath, "*.dll"))
        {
            Assembly assembly;
            try
            {
                assembly = Assembly.LoadFile(dllPath);
            }
            catch
            {
                // Skip invalid plugins
                continue;
            }

            foreach (var type in assembly.GetTypes().Where(t => typeof(ISystemMonitorPlugin).IsAssignableFrom(t) && t is { IsAbstract: false, IsInterface: false }))
            {
                try
                {
                    var instance = CreateSystemMonitorPluginOfType(type);
                    if (instance is IConfigurableSystemMonitorPlugin configurableSystemMonitorPlugin)
                    {
                        configurableSystemMonitorPlugin.Configure(config);
                    }
                    LoadedSystemMonitorPlugins.Add(instance);
                }
                catch 
                {
                    logger.Log(LogLevel.Error, $"Could not load system monitor plugin: {type.FullName}");
                }
            }
        }
    }

    private ISystemMonitorPlugin CreateSystemMonitorPluginOfType(Type type)
    {
        try
        {
            return  (ISystemMonitorPlugin)ActivatorUtilities.CreateInstance(serviceProvider, type);
        }
        catch
        {
            throw new Exception($"Cannot create instance of: {nameof(type.Name)}");
        }
    }
}