using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using NLog.Targets;
using System;

namespace NLogLoggerBuilder;

/// <summary>
/// DebugSystem extension methods
/// </summary>
public static class DebugSystemTargetExtensions
{
    /// <summary>
    /// Add a debug system target
    /// </summary>
    /// <param name="builder">Builder</param>
    /// <param name="name">Target's name</param>
    /// <param name="config">Target's configuration method</param>
    /// <param name="minLevel">Minimum log level</param>
    /// <param name="maxLevel">Maximum log level</param>
    /// <returns><see cref="NLogLoggerProviderBuilder"/></returns>
    public static NLogLoggerProviderBuilder AddDebugSystemTarget(
        this NLogLoggerProviderBuilder builder,
        string name,
        Action<DebugSystemTarget> config,
        LogLevel minLevel = LogLevel.Trace,
        LogLevel maxLevel = LogLevel.Critical)
        => builder.AddTargetWithConfiguration(new DebugSystemTarget(name), config, minLevel, maxLevel);
}