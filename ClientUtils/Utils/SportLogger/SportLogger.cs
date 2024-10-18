using System;
using System.Threading.Tasks;
using Serilog.Core;

namespace ClientUtilsProject.Utils;

public class SportLogger : ISportLogger, IDisposable, IAsyncDisposable
{
    private Logger MyLogger;
    
    public void Verbose(string message)
    {
        MyLogger.Verbose(message);
    }
    
    public void Information(string message)
    {
        MyLogger.Information(message);
    }

    public SportLogger(Logger logger)
    {
        MyLogger = logger;
    }

    public async void Dispose()
    {
        await DisposeAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await MyLogger.DisposeAsync();
    }
}