using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using NLog.Targets;
using System;

namespace NLogLoggerBuilder;

/// <summary>
/// FileTarget extension methods
/// </summary>
public static class FileTargetExtensions
{
    /// <summary>
    /// Add a file target
    /// </summary>
    /// <param name="builder">Builder</param>
    /// <param name="name">Target name</param>
    /// <param name="config">Target's configuration method</param>
    /// <param name="minLevel">Minimum log level</param>
    /// <param name="maxLevel">Maximum log level</param>
    /// <returns><see cref="NLogLoggerProviderBuilder"/></returns>
    public static NLogLoggerProviderBuilder AddFileTarget(
        this NLogLoggerProviderBuilder builder,
        string name,
        Action<FileTarget> config,
        LogLevel minLevel = LogLevel.Trace,
        LogLevel maxLevel = LogLevel.Critical)
        => builder.AddTargetWithConfiguration(new FileTarget(name), config, minLevel, maxLevel);
}