using System;
using System.Linq;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;


namespace NLogLoggerBuilder;

/// <summary>
/// Builder class that creates loggers of type <see cref="ILogger"/> with NLog
/// </summary>
public class NLogLoggerProviderBuilder
{
    private readonly LogFactory _logFactory = new();
    private readonly LoggingConfiguration _loggingConfiguration = new();

    /// <summary>
    /// Gets the default layout
    /// </summary>
    public static string DefaultLayout => "${longdate} | ${level:uppercase=true} | ${message} | ThreadId=${threadid} | ${callsite:className=false:fileName=true:includeSourcePath=false:methodName=true} | GCTotalMemory=${gc:property=TotalMemory}${onexception:inner= | ${exception:format=ToString}}";

    /// <summary>
    /// Build an <see cref="ILogger"/> with added targets
    /// </summary>
    /// <param name="name">Logger name</param>
    /// <returns><see cref="ILogger"/></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public ILogger BuildLogger(string name)
    {
        if (!_loggingConfiguration.AllTargets.Any())
            throw new InvalidOperationException("No target has been added");
        
        _logFactory.Setup().LoadConfiguration(_loggingConfiguration);
        var provider = new NLogLoggerProvider(new NLogProviderOptions(), _logFactory);
        return provider.CreateLogger(name);
    }

    /// <summary>
    /// Build an <see cref="ILogger"/> from NLog configuration file
    /// </summary>
    /// <param name="name">Logger name</param>
    /// <param name="configFilePath">NLog configuration file path</param>
    /// <returns><see cref="ILogger"/></returns>
    public ILogger BuildLoggerFromConfigFile(string name, string configFilePath)
    {
        _logFactory.Setup().LoadConfigurationFromFile(configFilePath);
        var provider = new NLogLoggerProvider(new NLogProviderOptions(), _logFactory);
        return provider.CreateLogger(name);
    }

    /// <summary>
    /// Add a target and create a rule for it
    /// </summary>
    /// <param name="target">Target to register</param>
    /// <param name="minLevel">Minimum log level</param>
    /// <param name="maxLevel">Maximum log level</param>
    /// <typeparam name="T">Target type</typeparam>
    public NLogLoggerProviderBuilder AddTarget<T>(
        T target,
        LogLevel minLevel = LogLevel.Trace,
        LogLevel maxLevel = LogLevel.Critical)
        where T : Target
    {
        _loggingConfiguration.AddTarget(target); 
        
        var rule = new LoggingRule(
            "*",
            ConvertToNLogLevel(minLevel),
            ConvertToNLogLevel(maxLevel),
            target);
        
        _loggingConfiguration.AddRule(rule);

        return this;
    }

    /// <summary>
    /// Configure a target, add it and create a rule for it
    /// </summary>
    /// <param name="target">Target to register</param>
    /// <param name="config">Target's configuration method</param>
    /// <param name="minLevel">Minimum log level</param>
    /// <param name="maxLevel">Maximum log level</param>
    /// <typeparam name="T">Target type</typeparam>
    public NLogLoggerProviderBuilder AddTargetWithConfiguration<T>(
        T target,
        Action<T> config,
        LogLevel minLevel = LogLevel.Trace,
        LogLevel maxLevel = LogLevel.Critical)
        where T : Target
    {
        config?.Invoke(target);
        return AddTarget(target, minLevel, maxLevel);
    }
    
    private static NLog.LogLevel ConvertToNLogLevel(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace       => NLog.LogLevel.Trace,
            LogLevel.Debug       => NLog.LogLevel.Debug,
            LogLevel.Information => NLog.LogLevel.Info,
            LogLevel.Warning     => NLog.LogLevel.Warn,
            LogLevel.Error       => NLog.LogLevel.Error,
            LogLevel.Critical    => NLog.LogLevel.Fatal,
            LogLevel.None        => NLog.LogLevel.Off,
            _                    => NLog.LogLevel.Off
        };
    }
}