using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientUtilsProject.DataClasses;

public partial class Session : ObservableObject
{

    [ObservableProperty]
    public ObservableCollection<SessionExercice> _sessionItems;

    



}
