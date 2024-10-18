using ClientUtilsProject.Utils;
using SportProject.ThemeChanges;

[assembly: Dependency(typeof(ThemeDetector))]
namespace SportProject.ThemeChanges;

public class ThemeDetector : IThemeDetector
{
    public AppTheme GetDeviceTheme()
    {
        var uiMode = (Android.App.Application.Context.Resources.Configuration.UiMode &
                      Android.Content.Res.UiMode.NightMask);
        return uiMode == Android.Content.Res.UiMode.NightYes ? AppTheme.Dark : AppTheme.Light;
    }
}