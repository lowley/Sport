using Android.App;
using Android.Content.PM;
using Android.OS;

namespace WIPClient2
{
    [Activity(Theme = "@style/Maui.SplashTheme", Name = "com.companyname.wipclient2.MainActivity", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
    }
}
