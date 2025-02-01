using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLogLoggerBuilder;
using NLog.Extensions.Logging;

namespace Tester;

public static class Program
{
    public static IServiceProvider? ServiceProvider { get; private set; }
    
    public static void Main(string[] args)
    {
        try
        {
            ConfigureServices();

            var dummy = ServiceProvider!.GetRequiredService<Dummy>();

            var logger = ServiceProvider!.GetRequiredService<NLogLoggerProviderBuilder>()
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
                .BuildLogger("myLogger");
            var logger2 = ServiceProvider!.GetRequiredService<NLogLoggerProviderBuilder>()
                .AddConsoleTarget("myConsoleTarget",
                    config =>
                    {
                        config.Layout = NLogLoggerProviderBuilder.DefaultLayout;
                    })
                .BuildLogger("myLogger");
            
            logger.LogCritical("This is a critical message");
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
        ServiceProvider = services.BuildServiceProvider();
    }
}