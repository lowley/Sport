using System.Collections.ObjectModel;
using ClientUtilsProject.DataClasses;
using ClientUtilsProject.Utils;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientUtilsProject.ViewModels;

public partial class SessionsVM : ObservableObject
{
    [ObservableProperty] public static ObservableCollection<Session> _sessions;

    private SportContext Context { get; set; }
    private ISportNavigation Navigation { get; set; }
    private ISportLogger Logger { get; set; }



    
    
    public SessionsVM(
        ISportNavigation navigation, 
        SportContext context,
        ISportLogger logger)
    {
        Navigation = navigation;
        Context = context;
        Logger = logger;
    }
}