using Android.Views;
using Microsoft.Maui.Platform;

namespace SportProject;

public class StatusBarBehavior : Behavior<ContentPage>
{
    public static readonly BindableProperty StatusBarColorProperty =
        BindableProperty.Create(nameof(StatusBarColor), typeof(Color), typeof(StatusBarBehavior), Colors.Black);

    public Color StatusBarColor
    {
        get => (Color)GetValue(StatusBarColorProperty);
        set => SetValue(StatusBarColorProperty, value);
    }

    protected override void OnAttachedTo(ContentPage bindable)
    {
        base.OnAttachedTo(bindable);
        DefineColor(Application.Current.RequestedTheme);

        Application.Current.RequestedThemeChanged += (s, a) => { DefineColor(a.RequestedTheme); };
    }

    private void DefineColor(AppTheme theme)
    {
        if (Application.Current.Resources.MergedDictionaries.SelectMany(d => d.Keys).Contains("SixtyColor")
            && Application.Current.Resources.MergedDictionaries.SelectMany(d => d.Keys).Contains("SixtyColorDark"))
        {
            Application.Current.Resources.MergedDictionaries.First(d => d.Keys.Contains("SixtyColor"))
                .TryGetValue("SixtyColor", out var valeur);
            Application.Current.Resources.MergedDictionaries.First(d => d.Keys.Contains("SixtyColorDark"))
                .TryGetValue("SixtyColorDark", out var valeurDark);

            StatusBarColor = theme == AppTheme.Light ? (Color)valeur! : (Color)valeurDark!;
            UpdateStatusBarColor();
        }
    }

    protected override void OnDetachingFrom(ContentPage bindable)
    {
        base.OnDetachingFrom(bindable);
    }

    private void UpdateStatusBarColor()
    {
        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            var window = Platform.CurrentActivity.Window;
            window.SetStatusBarColor(StatusBarColor.ToPlatform());
            window.SetNavigationBarColor(StatusBarColor.ToPlatform());
        }
    }
}