using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using Serilog;
using Sport.Pages;
using Sport.VM;
using Serilog.Core;

namespace Sport
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var log = SetupSerilog();

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton(log);
            builder.Services.AddTransient<HomeVM>();
            builder.Services.AddTransient<HomePage>();

            return builder.Build();
        }

        public static Logger SetupSerilog()
        {
            var flushInterval = new TimeSpan(0, 0, 1);
            var file = Path.Combine(FileSystem.Current.AppDataDirectory, "logs-.txt");

            return new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Filter.ByExcluding(le =>
                false)
            .WriteTo.File(file, flushToDiskInterval: flushInterval, encoding: System.Text.Encoding.UTF8, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
            .WriteTo.Debug()

            .CreateLogger();
        }
    }
}
