using System.Collections.ObjectModel;
using ClientUtilsProject.DataClasses;
using ClientUtilsProject.Utils;
using ClientUtilsProject.Utils.SportRepository;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace ClientUtilsProject.ViewModels;

public partial class ExercisesVM : ObservableObject
{
    [ObservableProperty] public static ObservableCollection<Exercise> _exercices = [];
    
    private ISportRepository Repo { get; set; }
    private ISportNavigation Navigation { get; set; }
    private ISportLogger Logger { get; set; }
    
    public async Task LoadExercises()
    {
        var exercises = Repo.Query<Exercise>()
            .Include(e => e.ExerciseDifficulties)
            .OrderBy(e => e.ExerciseName)
            .ToList();
        Exercices = new ObservableCollection<Exercise>(exercises);
    }

    public ExercisesVM(
        ISportNavigation navigation, 
        ISportRepository repo,
        ISportLogger logger)
    {
        Navigation = navigation;
        Repo = repo;
        Logger = logger;


    }

}
