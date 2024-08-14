using System.Collections.ObjectModel;
using ClientUtilsProject.DataClasses;
using ClientUtilsProject.Utils;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientUtilsProject.ViewModels;

public partial class ExercisesVM : ObservableObject
{
    [ObservableProperty] public static ObservableCollection<Exercise> _exercices = [];
    
    private SportContext Context { get; set; }
    private ISportNavigation Navigation { get; set; }
    private ISportLogger Logger { get; set; }

    public ExercisesVM(
        ISportNavigation navigation, 
        SportContext context,
        ISportLogger logger)
    {
        Navigation = navigation;
        Context = context;
        Logger = logger;


    }
}
