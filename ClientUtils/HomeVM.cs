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

    public HomeVM()
    {
        //CustomPins.ListChanged += (object? sender, ListChangedEventArgs e) => OnPropertyChanged(nameof(CustomPins));

    }
}

