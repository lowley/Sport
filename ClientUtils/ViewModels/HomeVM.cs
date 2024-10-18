using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ClientUtilsProject.Utils;
using ClientUtilsProject.Utils.SportRepository;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Syncfusion.Maui.Buttons;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace ClientUtilsProject.ViewModels;

public partial class HomeVM : ObservableObject
{
    private ISportNavigation Navigation { get; set; }
    private ISportRepository Repo { get; set; }

    [ObservableProperty] public bool? _isDarkThemeRequired = null;

    private EventHandler<AppThemeChangedEventArgs> SelectThemeFromSystem = null;
    
    
    [RelayCommand]
    public async Task AddSession()
    {
        await Navigation.NavigateTo("sessions/session");
    }

    [RelayCommand]
    public async Task AddExercise()
    {
        await Navigation.NavigateTo("exercises/exercise");
    }

    [RelayCommand]
    public async Task ListExercises()
    {
        await Navigation.NavigateTo("exercises/exercises");
    }

    [RelayCommand]
    public async Task ListSessions()
    {
        await Navigation.NavigateTo("sessions/sessions");
    }

    [RelayCommand]
    public async Task Clear()
    {
        await Repo.Clear();
    }

    public HomeVM(ISportNavigation navigation, ISportRepository repo)
    {
        Navigation = navigation;
        Repo = repo;
        
        // SelectThemeFromSystem = (s, a) => 
        // {
        // if (a.RequestedTheme == AppTheme.Dark)
        // {
        //     Application.Current!.UserAppTheme = AppTheme.Dark;
        // }
        // else
        // {
        //     Application.Current!.UserAppTheme = AppTheme.Light;
        // }
        // };
        // Application.Current.RequestedThemeChanged += (sender, args) =>
        // {
        //     Console.WriteLine("fgd");
        //     Application.Current!.RequestedThemeChanged += SelectThemeFromSystem;
        // };
        // Application.Current.UserAppTheme = AppTheme.Unspecified;
    }
}