using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientUtilsProject.DataClasses;

public partial class Session : ObservableObject
{
    [ObservableProperty]
    private DateTime _sessionStartDate = DateTime.Now;

    [ObservableProperty]
    private TimeSpan _sessionStartTime = DateTime.Now.TimeOfDay;

    [ObservableProperty]
    private TimeSpan _sessionEndTime = DateTime.Now.TimeOfDay;
    
    [ObservableProperty]
    private ObservableCollection<SessionExercice> _sessionItems;


    public Session()
    {
        _sessionItems = [];
    }
}
