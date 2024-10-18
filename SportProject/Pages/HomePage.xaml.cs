using ClientUtilsProject.ViewModels;
using Microsoft.Maui.Platform;
using Serilog.Core;
using SportProject.ThemeChanges;
using Syncfusion.Maui.Buttons;
using Application = Microsoft.Maui.Controls.Application;

namespace SportProject.Pages;

public partial class HomePage : ContentPage
{
    public HomeVM VM { get; set; }
    private Logger Logger { get; set; }

    private ThemeChangeDetector themeChangeDetector;

    public HomePage(HomeVM vm, Logger logger)
    {
        InitializeComponent();
        themeChangeDetector = new ThemeChangeDetector().Start();
        VM = vm;
        BindingContext = VM;
        Logger = logger;

        Application.Current!.UserAppTheme = AppTheme.Unspecified;
        themeChangeDetector.ThemeChanged += OnThemeChangedWithDark;
        // Application.Current!.UserAppTheme = AppTheme.Light;
        // OnThemeChanged(true);
    }

    private void SfSwitch_OnStateChanged(object? sender, SwitchStateChangedEventArgs e)
    {
        //OnThemeChanged(Application.Current!.UserAppTheme == AppTheme.Light ? true : false);

        switch (e.NewValue)
        {
            case null:
                themeChangeDetector.ThemeChanged += OnThemeChangedWithDark;
                themeChangeDetector.Start();
                themeChangeDetector.UseCurrent();
                break;
            case true:
                themeChangeDetector.Stop();
                themeChangeDetector.ThemeChanged -= OnThemeChangedWithDark;
                OnThemeChangedWithDark(true);
                break;
            case false:
                themeChangeDetector.Stop();
                themeChangeDetector.ThemeChanged -= OnThemeChangedWithDark;
                OnThemeChangedWithDark(false);
                break;
        }
    }

    private void OnThemeChangedWithDark(bool isDark)
    {
        ICollection<ResourceDictionary> mergedDictionaries = Application.Current!.Resources.MergedDictionaries;

        var sixtyColor = GetColorByName(mergedDictionaries, "SixtyColor");
        var sixtyColorDark = GetColorByName(mergedDictionaries, "SixtyColorDark");
        var textColor = GetColorByName(mergedDictionaries, "TextColor");
        var textColorDark = GetColorByName(mergedDictionaries, "TextColorDark");

        if (isDark)
        {
            Application.Current!.UserAppTheme = AppTheme.Dark;
            if (sixtyColorDark is not null)
            {
                ModifyShellWith(mergedDictionaries, sixtyColorDark, textColorDark);
            }
        }
        else
        {
            Application.Current!.UserAppTheme = AppTheme.Light;
            if (sixtyColor is not null)
            {
                ModifyShellWith(mergedDictionaries, sixtyColor, textColor);
            }
        }
    }

    private Color? GetColorByName(ICollection<ResourceDictionary> dicos, string colorKey)
    {
        object? objectFound = null;
        dicos.FirstOrDefault(rd => rd.Keys.Contains(colorKey))
            ?.TryGetValue(colorKey, out objectFound);
        return (Color?)objectFound;
    }

    private void ModifyShellWith(ICollection<ResourceDictionary> dicos, Color backgroundColor, Color textColor)
    {
        UpdateStatusNavigationBarColor(backgroundColor);
        
        Shell.SetBackgroundColor(Shell.Current, backgroundColor);
        Shell.SetTabBarBackgroundColor(Shell.Current, backgroundColor);
        
        var shellStyle = new Style(typeof(Shell));
        shellStyle.Setters.Add(new Setter()
        {
            Property = Shell.TabBarBackgroundColorProperty,
            Value = backgroundColor
        });
        shellStyle.Setters.Add(new Setter()
        {
            Property = Shell.TabBarTitleColorProperty,
            Value = textColor
        });
        shellStyle.Setters.Add(new Setter()
        {
            Property = Shell.TabBarForegroundColorProperty,
            Value = textColor
        });
        shellStyle.Setters.Add(new Setter()
        {
            Property = Shell.TabBarUnselectedColorProperty,
            Value = textColor
        });
        shellStyle.Setters.Add(new Setter()
        {
            Property = Shell.BackgroundColorProperty,
            Value = backgroundColor
        });
        shellStyle.Setters.Add(new Setter()
        {
            Property = Shell.ForegroundColorProperty,
            Value = textColor
        });
        shellStyle.Setters.Add(new Setter()
        {
            Property = Shell.UnselectedColorProperty,
            Value = textColor
        });
        shellStyle.Setters.Add(new Setter()
        {
            Property = Shell.DisabledColorProperty,
            Value = textColor
        });
        shellStyle.Setters.Add(new Setter()
        {
            Property = Shell.TitleColorProperty,
            Value = textColor
        });
        dicos.Add(new() { shellStyle });
    }
    
    private void UpdateStatusNavigationBarColor(Color color)
    {
        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            var window = Platform.CurrentActivity.Window;
            window.SetStatusBarColor(color.ToPlatform());
            window.SetNavigationBarColor(color.ToPlatform());
        }
    }
}