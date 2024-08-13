using ClientUtilsProject.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ClientUtilsProject.ViewModels;

public partial class HomeVM : ObservableObject
{
    private ISportNavigation Navigation { get; set; }

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
    public void Clear()
    {
        ExercisesVM._exercices.Clear();
    }

    public HomeVM(ISportNavigation navigation)
    {
        Navigation = navigation;

    }
}

