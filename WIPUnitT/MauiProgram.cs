using CommunityToolkit.Maui;

namespace WIPUnitT
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp
                .CreateBuilder()
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseShinyFramework(
                    new DryIocContainerExtension(),
                    prism => prism.CreateWindow("NavigationPage/MainPage"),
                    new(
#if DEBUG
                        ErrorAlertType.FullError
#else
                        ErrorAlertType.NoLocalize
#endif
                    )
                )
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.SetMinimumLevel(LogLevel.Trace);
            builder.Logging.AddDebug();
#endif
            builder.Services.AddDataAnnotationValidation();

            builder.Services.RegisterForNavigation<MainPage, MainViewModel>();
            var app = builder.Build();

            return app;
        }
    }
}