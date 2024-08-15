using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientUtilsProject.DataClasses;

public partial class Session : SportEntity
{
    public Guid Id { get; set; }

    [ObservableProperty]
    public DateTime _sessionStartDate = DateTime.Now;

    [ObservableProperty]
    public TimeSpan _sessionStartTime = DateTime.Now.TimeOfDay;

    [ObservableProperty]
    public TimeSpan _sessionEndTime = DateTime.Now.TimeOfDay;
    
    [ObservableProperty]
    public ObservableCollection<SessionExercice> _sessionItems;


    public Session()
    {
        Id = Guid.NewGuid();
        _sessionItems = [];
    }
}
