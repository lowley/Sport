using ClientUtilsProject.Utils;
using ClientUtilsProject.Utils.SportRepository;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ClientUtilsProject.ViewModels;

public partial class HomeVM : ObservableObject
{
    private ISportNavigation Navigation { get; set; }
    private ISportRepository Repo { get; set; }
    
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
    }
}

