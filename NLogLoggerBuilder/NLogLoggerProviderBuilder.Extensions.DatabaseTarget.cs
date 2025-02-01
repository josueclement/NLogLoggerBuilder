using System;
using NLog.Targets;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace NLogLoggerBuilder;

public static class DatabaseTargetExtensions
{
    /// <summary>
    /// Add a database target
    /// </summary>
    /// <param name="builder">Builder</param>
    /// <param name="name">Target name</param>
    /// <param name="config">Target's configuration method</param>
    /// <param name="minLevel">Minimum log level</param>
    /// <param name="maxLevel">Maximum log level</param>
    /// <returns><see cref="NLogLoggerProviderBuilder"/></returns>
    public static NLogLoggerProviderBuilder AddDatabaseTarget(
        this NLogLoggerProviderBuilder builder,
        string name,
        Action<DatabaseTarget> config,
        LogLevel minLevel = LogLevel.Trace,
        LogLevel maxLevel = LogLevel.Critical)
        => builder.AddTargetWithConfiguration(new DatabaseTarget(name), config, minLevel, maxLevel);
}