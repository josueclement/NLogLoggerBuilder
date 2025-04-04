using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using NLog.Targets;
using System;

namespace NLogLoggerBuilder;

/// <summary>
/// Colored console extension methods
/// </summary>
public static class ColoredConsoleTargetExtensions
{
    /// <summary>
    /// Add a colored console target
    /// </summary>
    /// <param name="builder">Builder</param>
    /// <param name="name">Target's name</param>
    /// <param name="config">Target's configuration method</param>
    /// <param name="minLevel">Minimum log level</param>
    /// <param name="maxLevel">Maximum log level</param>
    /// <returns><see cref="NLogLoggerProviderBuilder"/></returns>
    public static NLogLoggerProviderBuilder AddColoredConsoleTarget(
        this NLogLoggerProviderBuilder builder,
        string name,
        Action<ColoredConsoleTarget> config,
        LogLevel minLevel = LogLevel.Trace,
        LogLevel maxLevel = LogLevel.Critical)
        => builder.AddTargetWithConfiguration(new ColoredConsoleTarget(name), config, minLevel, maxLevel);
}