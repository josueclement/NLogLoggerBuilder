using Microsoft.Extensions.Logging;
using NLogLoggerBuilder;


namespace Tester;

public class Dummy
{
    public Dummy(ILogger<Dummy> logger,
        NLogLoggerProviderBuilder builder1,
        NLogLoggerProviderBuilder builder2)
    {
        
    }
}