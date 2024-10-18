using System.Timers;
using LanguageExt.Pipes;

namespace SportProject.ThemeChanges;

public class ThemeChangeDetector
{
    public event Action<bool> ThemeChanged;
    private System.Timers.Timer timer;
    public ThemeDetector detector;

    public ThemeChangeDetector()
    {
        detector = new();
        timer = new(1000);
        timer.Elapsed += Do;
        timer.AutoReset = true;
    }

    public ThemeChangeDetector Start()
    {
        timer.Start();
        return this;
    }

    private void Do(Object source, ElapsedEventArgs args)
    {
        UseCurrent();
    }

    public void UseCurrent()
    {
        bool isDark = detector.GetDeviceTheme() == AppTheme.Dark;

        // Émettre l'événement si le thème a changé
        ThemeChanged?.Invoke(isDark);
    }

    public void Stop()
        => timer.Stop();
}