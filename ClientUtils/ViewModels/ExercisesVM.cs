using System.Collections.ObjectModel;
using ClientUtilsProject.DataClasses;
using ClientUtilsProject.Utils;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientUtilsProject.ViewModels;

public partial class ExercisesVM : ObservableObject
{
    [ObservableProperty] public static ObservableCollection<Exercise> _exercices = [];

    private ISportNavigation Navigation { get; set; }


    public ExercisesVM(ISportNavigation navigation)
    {
        Navigation = navigation;

    }
}
