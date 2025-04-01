using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLogLoggerBuilder;
using NLog.Extensions.Logging;
using NLog.Targets;

namespace Tester;

public static class Program
{
    private static IServiceProvider? _serviceProvider;

    private static IServiceProvider ServiceProvider => _serviceProvider ??
                                                      throw new InvalidOperationException("ServiceProvider is not initialized.");

    private static void ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddNLog();
        });
        services.AddSingleton<Dummy>();
        services.AddTransient<NLogLoggerProviderBuilder>();
        _serviceProvider = services.BuildServiceProvider();
    }
    
    public static void Main(string[] args)
    {
        try
        {
            ConfigureServices();

            var dummy = ServiceProvider.GetRequiredService<Dummy>();

var logger = ServiceProvider.GetRequiredService<NLogLoggerProviderBuilder>()
    .AddColoredConsoleTarget("myColoredConsoleTarget",
        config =>
        {
            config.Layout = NLogLoggerProviderBuilder.DefaultLayout;
        })
    .AddFileTarget("myFileTarget",
        config =>
        {
            config.Layout = NLogLoggerProviderBuilder.DefaultLayout;
            config.FileName = "Logs/tester.log";
        })
    .AddDatabaseTarget("myDatabaseTarget",
        config =>
        {
            // CREATE TABLE "LOGS" (
            //     "Id"	INTEGER,
            //     "Timestamp"	TEXT,
            //     "Level"	TEXT,
            //     "Message"	TEXT,
            //     "ThreadId"	INTEGER,
            //     "Callsite"	TEXT,
            //     "GCTotalMemory"	INTEGER,
            //     "Exception"	TEXT,
            //     PRIMARY KEY("Id" AUTOINCREMENT)
            // );
            config.KeepConnection = false;
            config.DBProvider = "System.Data.SQLite.SQLiteConnection, System.Data.SQLite";
            config.ConnectionString = "Data Source=C:\\Temp\\logs.db;Version=3;";
            config.CommandText = "INSERT INTO LOGS (Timestamp, Level, Message, ThreadId, Callsite, GCTotalMemory, Exception) VALUES (@Timestamp, @Level, @Message, @ThreadId, @Callsite, @GCTotalMemory, @Exception)";
            config.Parameters.Add(new DatabaseParameterInfo { Name = "@Timestamp", Layout = "${longdate}" });
            config.Parameters.Add(new DatabaseParameterInfo { Name = "@Level", Layout = "${level:uppercase=true}" });
            config.Parameters.Add(new DatabaseParameterInfo { Name = "@Message", Layout = "${message}" });
            config.Parameters.Add(new DatabaseParameterInfo { Name = "@ThreadId", Layout = "${threadid}" });
            config.Parameters.Add(new DatabaseParameterInfo { Name = "@Callsite", Layout = "${callsite:className=false:fileName=true:includeSourcePath=false:methodName=true}" });
            config.Parameters.Add(new DatabaseParameterInfo { Name = "@GCTotalMemory", Layout = "${gc:property=TotalMemory}" });
            config.Parameters.Add(new DatabaseParameterInfo { Name = "@Exception", Layout = "${exception:format=ToString}" });
        })
    .BuildLogger("myLogger");
            var logger2 = ServiceProvider.GetRequiredService<NLogLoggerProviderBuilder>()
                .AddConsoleTarget("myConsoleTarget",
                    config =>
                    {
                        config.Layout = NLogLoggerProviderBuilder.DefaultLayout;
                    })
                .BuildLogger("myLogger");

            try
            {
                A();
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "This is a critical message with value {value}", 12); 
            }
            
            logger2.LogCritical("This is a critical message");
            logger.LogError("This is an error message");
            logger.LogWarning("This is a warning message");
            logger.LogInformation("This is an information message");
            logger.LogDebug("This is a debug message");
            logger.LogTrace("This is a trace message");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private static void A() => B();
    private static void B() => C();
    private static void C() => throw new InvalidOperationException("Something failed...");
}