# NLogLoggerBuilder

NLogLoggerBuilder is a .NET library that build NLog logger for Microsoft.Extensions.Logging 

Register service :

```csharp
services.AddLogging(builder =>
{
    builder.ClearProviders();
    builder.AddNLog();
});
services.AddTransient<NLogLoggerProviderBuilder>();
```

Build logger with multiple targets :

```csharp
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
            // Table structure for SQLite : 
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
```