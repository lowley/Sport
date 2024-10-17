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
        DefineColor();

        Application.Current.RequestedThemeChanged += (s, a) => { DefineColor(); };
    }

    private void DefineColor()
    {
        if (Application.Current.Resources.MergedDictionaries.Any(d => d.ContainsKey("SixtyColor")))
        {
            Application.Current.Resources.MergedDictionaries.First(d => d.ContainsKey("SixtyColor"))
                .TryGetValue("SixtyColor", out var valeur);

            StatusBarColor = (Color)valeur;
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