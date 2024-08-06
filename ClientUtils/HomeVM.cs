using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace Sport.VM;

public partial class HomeVM : ObservableObject
{
    [RelayCommand]
    public async Task AddSession()
    {
        await Shell.Current.GoToAsync("sessions/session", false);
    }

    [RelayCommand]
    public async Task AddExercise()
    {
        await Shell.Current.GoToAsync("exercises/exercise", false);
    }

    [RelayCommand]
    public async Task ListExercises()
    {
        await Shell.Current.GoToAsync("exercises/exercises", false);
    }

    public HomeVM()
    {

    }
}

