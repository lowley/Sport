using Android.App;
using Android.Content.PM;
using Android.OS;

namespace WIPClient
{
    [Activity(Theme = "@style/Maui.SplashTheme", Name = "sxb.sport.MainActivity", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Ajouter une variable d'environnement
            Java.Lang.JavaSystem.SetProperty("DX.UITESTINGENABLED", "True");
        }
        
        
    }
}
