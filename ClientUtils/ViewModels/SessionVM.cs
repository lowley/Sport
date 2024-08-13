using ClientUtilsProject.DataClasses;
using ClientUtilsProject.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ClientUtilsProject.ViewModels;

public partial class SessionVM : ObservableObject
{
    [ObservableProperty] private Session _session;
    private ISportNavigation Navigation { get; set; }

    [RelayCommand]
    public async Task CloseSession()
    {
        Session.SessionEndTime = DateTime.Now.TimeOfDay;

        // if (Session.SessionItems.Any())
        // {
        // }

        SessionsVM._sessions.Add(Session);

        await Navigation.NavigateBack();
    }

    public SessionVM(ISportNavigation navigation)
    {
        Navigation = navigation;
        Session = new Session();
    }
}