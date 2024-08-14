using ClientUtilsProject.DataClasses;
using ClientUtilsProject.Utils;
using ClientUtilsProject.Utils.SportRepository;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using Serilog;
using Sport.Pages;
using ClientUtilsProject.ViewModels;
using Serilog.Core;
using DevExpress.Maui;
using Sport.Converters;
using SportProject.Pages;

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
                .UseDevExpress()
                .UseDevExpressEditors()
                .UseDevExpressControls()
                .UseDevExpressCollectionView()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("gauge.ttf", "FontelloGauge");

                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton(log);
            
            builder.Services.AddTransient<HomeVM>();
            builder.Services.AddTransient<HomePage>();

            builder.Services.AddTransient<SessionVM>();
            builder.Services.AddTransient<SessionPage>();

            builder.Services.AddTransient<ExerciseVM>();
            builder.Services.AddTransient<ExercisePage>();

            builder.Services.AddTransient<ExercisesVM>();
            builder.Services.AddTransient<ExercisesPage>();

            builder.Services.AddTransient<SessionsVM>();
            builder.Services.AddTransient<SessionsPage>();

            builder.Services.AddTransient<ISportLogger, SportLogger>();
            builder.Services.AddTransient<ISportRepository, SportRepository>();
            builder.Services.AddTransient<ISportNavigation, SportNavigation>();
            //builder.Services.AddTransient<SportContext>();

            builder.Services.AddDbContext<SportContext>(contextLifetime: ServiceLifetime.Transient,
                optionsLifetime: ServiceLifetime.Transient);
            
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
