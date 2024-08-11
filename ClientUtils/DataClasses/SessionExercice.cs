using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientUtilsProject.DataClasses;

public partial class SessionExercice : ObservableObject
{
    [ObservableProperty]
    public Exercise _exercice;

    [ObservableProperty]
    public ObservableCollection<SessionExerciceSerie> _series;

    

}