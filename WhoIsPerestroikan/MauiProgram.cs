using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Syncfusion.Maui.Core.Hosting;
using CommunityToolkit.Maui;
using WhoIsPerestroikan.VM;
using Serilog.Events;
using Serilog;
using Serilog.Core;

namespace WhoIsPerestroikan
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            var log = SetupSerilog();

            builder
                .UseMauiApp<App>()
                .ConfigureSyncfusionCore()
                .UseMauiCommunityToolkit()
                .UseSkiaSharp(true)
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseMauiMaps()
                .ConfigureMauiHandlers(handlers =>
                {
                    handlers.AddHandler<Microsoft.Maui.Controls.Maps.Map, CustomMapHandler>();
                });

            builder.Services.AddSingleton(log);

            builder.Services.AddTransient<DisplayVM>();
            builder.Services.AddTransient<LocationService>();
            builder.Services.AddTransient<CommunicationWithServer>();
            builder.Services.AddTransient<DisplayPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

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
